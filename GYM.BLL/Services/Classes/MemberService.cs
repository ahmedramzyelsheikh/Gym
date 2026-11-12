using AutoMapper;
using GYM.BLL.Common;
using GYM.BLL.Services.Helpers;
using GYM.BLL.Services.Interfaces;
using GYM.BLL.ViewModels.Members;
using GYM.DAL.Data.Models;
using GYM.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYM.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private const string PhotoFolder = "Members";

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachments;

        public MemberService(IUnitOfWork uow, IMapper mapper, IAttachmentService attachments)
        {
            _uow = uow;
            _mapper = mapper;
            _attachments = attachments;
        }

        public async Task<Result<IEnumerable<MemberView>>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _uow.Repository<Member>().Query()
                                    .OrderByDescending(m => m.Id)
                                    .ToListAsync(ct);
            return Result<IEnumerable<MemberView>>.Success(_mapper.Map<IEnumerable<MemberView>>(members));
        }

        public async Task<Result<MemberView>> GetMemberAsync(int id, CancellationToken ct = default)
        {
            var member = await _uow.Repository<Member>().Query().FirstOrDefaultAsync(m => m.Id == id, ct);
            if (member is null) return Result<MemberView>.Failure("Member not found.");

            var view = _mapper.Map<MemberView>(member);

            var now = DateTime.Now;
            var active = await _uow.Repository<Membership>().Query()
                .Where(m => m.MemberId == id && m.EndDate > now)
                .FirstOrDefaultAsync(ct);
            if (active is not null)
            {
                view.MembershipStartDate = active.CreateAt.ToString("dd MMM yyyy");
                view.MembershipEndDate = active.EndDate.ToString("dd MMM yyyy");
            }

            return Result<MemberView>.Success(view);
        }

        public async Task<Result> CreateMemberAsynce(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var duplicate = await UserHelper.CheckDuplicateAsync(_uow, model.Email, model.Phone, excludeId: null, ct);
            if (duplicate is not null) return Result.Failure(duplicate);

            var upload = await _attachments.UploadAsync(model.PhotoFile!, PhotoFolder, ct);
            if (!upload.IsSuccess) return Result.Failure(upload.ErrorMessage);

            var member = _mapper.Map<Member>(model);
            member.Photo = upload.Data;

            try
            {
                await _uow.Repository<Member>().AddAsync(member, ct);
                await _uow.SaveChangesAsync(ct);
                return Result.Success();
            }
            catch (DbUpdateException)
            {
                _attachments.Delete(upload.Data, PhotoFolder);
                return Result.Failure("Could not save the member. Please check the data and try again.");
            }
        }

        public async Task<Result<HealthRecordViewModel>> GetDetailsHealthRecord(int memberId, CancellationToken ct = default)
        {
            var record = await _uow.Repository<HealthRecord>().Query()
                                   .FirstOrDefaultAsync(h => h.MemberId == memberId, ct);
            return record is null
                ? Result<HealthRecordViewModel>.Failure("No health record found for this member.")
                : Result<HealthRecordViewModel>.Success(_mapper.Map<HealthRecordViewModel>(record));
        }

        public async Task<Result<MemberToUpdateViewModel>> GetMemberToEditAsync(int id, CancellationToken ct = default)
        {
            var member = await _uow.Repository<Member>().Query().FirstOrDefaultAsync(m => m.Id == id, ct);
            return member is null
                ? Result<MemberToUpdateViewModel>.Failure("Member not found.")
                : Result<MemberToUpdateViewModel>.Success(_mapper.Map<MemberToUpdateViewModel>(member));
        }

        public async Task<Result> UpdateMemberAsync(MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var repo = _uow.Repository<Member>();
            var member = await repo.Query(tracking: true).FirstOrDefaultAsync(m => m.Id == model.Id, ct);
            if (member is null) return Result.Failure("Member not found.");

            var duplicate = await UserHelper.CheckDuplicateAsync(_uow, model.Email, model.Phone, excludeId: model.Id, ct);
            if (duplicate is not null) return Result.Failure(duplicate);

            string? oldPhoto = null;
            if (model.PhotoFile is not null)
            {
                var upload = await _attachments.UploadAsync(model.PhotoFile, PhotoFolder, ct);
                if (!upload.IsSuccess) return Result.Failure(upload.ErrorMessage);
                oldPhoto = member.Photo;
                member.Photo = upload.Data;
            }

            // Name, gender and date of birth are read-only on the edit screen.
            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address = new Address
            {
                BuildingNomber = model.BuildingNumber,
                City = model.City,
                Street = model.Street
            };

            await _uow.SaveChangesAsync(ct);
            _attachments.Delete(oldPhoto, PhotoFolder);
            return Result.Success();
        }

        public async Task<Result> DeleteMemberAsync(int id, CancellationToken ct = default)
        {
            var repo = _uow.Repository<Member>();
            var member = await repo.Query(tracking: true).FirstOrDefaultAsync(m => m.Id == id, ct);
            if (member is null) return Result.Failure("Member not found.");

            var now = DateTime.Now;
            var hasUpcomingBookings = await _uow.Repository<Booking>().Query()
                .AnyAsync(b => b.MemberId == id && b.Session.EndDate >= now, ct);
            if (hasUpcomingBookings)
                return Result.Failure("This member has upcoming or ongoing bookings. Cancel them first.");

            var photo = member.Photo;
            repo.Delete(member);
            await _uow.SaveChangesAsync(ct);
            _attachments.Delete(photo, PhotoFolder);
            return Result.Success();
        }
    }
}
