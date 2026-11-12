using AutoMapper;
using GYM.BLL.Common;
using GYM.BLL.Services.Helpers;
using GYM.BLL.Services.Interfaces.Attachments;
using GYM.BLL.Services.Interfaces.Trainers;
using GYM.BLL.ViewModels.Trainers;
using GYM.DAL.Data.Models;
using GYM.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYM.BLL.Services.Classes.Trainers
{
    public class TrainerService : ITrainerService
    {
        private const string PhotoFolder = "Trainers";

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachments;

        public TrainerService(IUnitOfWork uow, IMapper mapper, IAttachmentService attachments)
        {
            _uow = uow;
            _mapper = mapper;
            _attachments = attachments;
        }

        public async Task<Result<IEnumerable<TrainerView>>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _uow.Repository<Trainer>().Query().OrderBy(t => t.Name).ToListAsync(ct);
            return Result<IEnumerable<TrainerView>>.Success(_mapper.Map<IEnumerable<TrainerView>>(trainers));
        }

        public async Task<Result<TrainerView>> Details(int id, CancellationToken ct = default)
        {
            var trainer = await _uow.Repository<Trainer>().Query().FirstOrDefaultAsync(t => t.Id == id, ct);
            return trainer is null
                ? Result<TrainerView>.Failure("Trainer not found.")
                : Result<TrainerView>.Success(_mapper.Map<TrainerView>(trainer));
        }

        public async Task<Result> AddTrainer(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var duplicate = await UserHelper.CheckDuplicateAsync(_uow, model.Email, model.Phone, excludeId: null, ct);
            if (duplicate is not null) return Result.Failure(duplicate);

            string? photo = null;
            if (model.PhotoFile is not null)
            {
                var upload = await _attachments.UploadAsync(model.PhotoFile, PhotoFolder, ct);
                if (!upload.IsSuccess) return Result.Failure(upload.ErrorMessage);
                photo = upload.Data;
            }

            var trainer = _mapper.Map<Trainer>(model);
            trainer.Photo = photo;

            try
            {
                await _uow.Repository<Trainer>().AddAsync(trainer, ct);
                await _uow.SaveChangesAsync(ct);
                return Result.Success();
            }
            catch (DbUpdateException)
            {
                _attachments.Delete(photo, PhotoFolder);
                return Result.Failure("Could not save the trainer. Please check the data and try again.");
            }
        }

        public async Task<Result<EditTrainerViewModel>> GetTrainerToEdit(int id, CancellationToken ct = default)
        {
            var trainer = await _uow.Repository<Trainer>().Query().FirstOrDefaultAsync(t => t.Id == id, ct);
            return trainer is null
                ? Result<EditTrainerViewModel>.Failure("Trainer not found.")
                : Result<EditTrainerViewModel>.Success(_mapper.Map<EditTrainerViewModel>(trainer));
        }

        public async Task<Result> EditCompete(EditTrainerViewModel model, CancellationToken ct = default)
        {
            var trainer = await _uow.Repository<Trainer>().Query(tracking: true).FirstOrDefaultAsync(t => t.Id == model.Id, ct);
            if (trainer is null) return Result.Failure("Trainer not found.");

            var duplicate = await UserHelper.CheckDuplicateAsync(_uow, model.Email, model.Phone, excludeId: model.Id, ct);
            if (duplicate is not null) return Result.Failure(duplicate);

            string? oldPhoto = null;
            if (model.PhotoFile is not null)
            {
                var upload = await _attachments.UploadAsync(model.PhotoFile, PhotoFolder, ct);
                if (!upload.IsSuccess) return Result.Failure(upload.ErrorMessage);
                oldPhoto = trainer.Photo;
                trainer.Photo = upload.Data;
            }

            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Specialtiy = model.Specialties!.Value;
            trainer.Address = new Address
            {
                BuildingNomber = model.BuildingNumber,
                City = model.City,
                Street = model.Street
            };

            await _uow.SaveChangesAsync(ct);
            _attachments.Delete(oldPhoto, PhotoFolder);
            return Result.Success();
        }

        public async Task<Result> Delete(int id, CancellationToken ct = default)
        {
            var repo = _uow.Repository<Trainer>();
            var trainer = await repo.Query(tracking: true).FirstOrDefaultAsync(t => t.Id == id, ct);
            if (trainer is null) return Result.Failure("Trainer not found.");

            var hasSessions = await _uow.Repository<Session>().Query().AnyAsync(s => s.TrainerId == id, ct);
            if (hasSessions)
                return Result.Failure("This trainer has sessions. Reassign or delete them first.");

            var photo = trainer.Photo;
            repo.Delete(trainer);
            await _uow.SaveChangesAsync(ct);
            _attachments.Delete(photo, PhotoFolder);
            return Result.Success();
        }
    }
}
