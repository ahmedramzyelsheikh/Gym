using AutoMapper;
using GYM.BLL.Common;
using GYM.BLL.Services.Interfaces.Sessions;
using GYM.BLL.ViewModels.Sessions;
using GYM.BLL.ViewModels.Trainers;
using GYM.DAL.Data.Models;
using GYM.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYM.BLL.Services.Classes.Sessions
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        private IQueryable<Session> SessionsWithDetails(bool tracking = false)
            => _uow.Repository<Session>().Query(tracking)
                   .Include(s => s.Trainer)
                   .Include(s => s.Category)
                   .Include(s => s.bookings);

        public async Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await SessionsWithDetails().OrderByDescending(s => s.StartDate).ToListAsync(ct);
            return Result<IEnumerable<SessionViewModel>>.Success(_mapper.Map<IEnumerable<SessionViewModel>>(sessions));
        }

        public async Task<Result<SessionViewModel>> GetSessionDetailsAsync(int id, CancellationToken ct = default)
        {
            var session = await SessionsWithDetails().FirstOrDefaultAsync(s => s.Id == id, ct);
            return session is null
                ? Result<SessionViewModel>.Failure("Session not found.")
                : Result<SessionViewModel>.Success(_mapper.Map<SessionViewModel>(session));
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            var error = ValidateDates(model.StartDate, model.EndDate)
                        ?? await ValidateTrainerAsync(model.TrainerId, model.StartDate, model.EndDate, excludeSessionId: null, ct);
            if (error is not null) return Result.Failure(error);

            if (!await _uow.Repository<Category>().Query().AnyAsync(c => c.Id == model.CategoryId, ct))
                return Result.Failure("Selected category does not exist.");

            var session = _mapper.Map<Session>(model);
            await _uow.Repository<Session>().AddAsync(session, ct);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToEditAsync(int id, CancellationToken ct = default)
        {
            var session = await _uow.Repository<Session>().Query().FirstOrDefaultAsync(s => s.Id == id, ct);
            if (session is null) return Result<UpdateSessionViewModel>.Failure("Session not found.");
            if (session.StartDate <= DateTime.Now)
                return Result<UpdateSessionViewModel>.Failure("Only upcoming sessions can be edited.");

            return Result<UpdateSessionViewModel>.Success(_mapper.Map<UpdateSessionViewModel>(session));
        }

        public async Task<Result> UpdateSessionAsync(UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _uow.Repository<Session>().Query(tracking: true).FirstOrDefaultAsync(s => s.Id == model.Id, ct);
            if (session is null) return Result.Failure("Session not found.");
            if (session.StartDate <= DateTime.Now) return Result.Failure("Only upcoming sessions can be edited.");

            var error = ValidateDates(model.StartDate, model.EndDate)
                        ?? await ValidateTrainerAsync(model.TrainerId, model.StartDate, model.EndDate, excludeSessionId: model.Id, ct);
            if (error is not null) return Result.Failure(error);

            session.Description = model.Description;
            session.TrainerId = model.TrainerId;
            session.StartDate = model.StartDate;
            session.EndDate = model.EndDate;

            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default)
        {
            var session = await SessionsWithDetails(tracking: true).FirstOrDefaultAsync(s => s.Id == id, ct);
            if (session is null) return Result.Failure("Session not found.");

            var now = DateTime.Now;
            if (session.StartDate <= now && session.EndDate >= now)
                return Result.Failure("You can't delete a session that is in progress.");
            if (session.StartDate > now && session.bookings.Any())
                return Result.Failure("This upcoming session has bookings. Cancel them first.");

            _uow.Repository<Session>().Delete(session);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result<IEnumerable<CategoryViewModel>>> GetCategoriesAsync(CancellationToken ct = default)
        {
            var categories = await _uow.Repository<Category>().Query().OrderBy(c => c.CategoryName).ToListAsync(ct);
            return Result<IEnumerable<CategoryViewModel>>.Success(_mapper.Map<IEnumerable<CategoryViewModel>>(categories));
        }

        public async Task<Result<IEnumerable<TrainerView>>> GetTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _uow.Repository<Trainer>().Query().OrderBy(t => t.Name).ToListAsync(ct);
            return Result<IEnumerable<TrainerView>>.Success(_mapper.Map<IEnumerable<TrainerView>>(trainers));
        }

        private static string? ValidateDates(DateTime start, DateTime end)
        {
            if (start <= DateTime.Now) return "Start date must be in the future.";
            if (end <= start) return "End date must be after the start date.";
            return null;
        }

        // The trainer must exist and must not have another session at the same time.
        private async Task<string?> ValidateTrainerAsync(int trainerId, DateTime start, DateTime end, int? excludeSessionId, CancellationToken ct)
        {
            if (!await _uow.Repository<Trainer>().Query().AnyAsync(t => t.Id == trainerId, ct))
                return "Selected trainer does not exist.";

            var overlaps = await _uow.Repository<Session>().Query()
                .AnyAsync(s => s.TrainerId == trainerId
                               && (!excludeSessionId.HasValue || s.Id != excludeSessionId.Value)
                               && s.StartDate < end && start < s.EndDate, ct);

            return overlaps ? "This trainer already has a session during this time." : null;
        }
    }
}
