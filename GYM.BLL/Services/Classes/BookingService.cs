using AutoMapper;
using GYM.BLL.Common;
using GYM.BLL.Services.Interfaces;
using GYM.BLL.ViewModels;
using GYM.DAL.Data.Models;
using GYM.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYM.BLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // Upcoming and ongoing sessions (completed ones can't be managed anymore).
        public async Task<Result<IEnumerable<SessionViewModel>>> GetManageableSessionsAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var sessions = await _uow.Repository<Session>().Query()
                .Include(s => s.Trainer)
                .Include(s => s.Category)
                .Include(s => s.bookings)
                .Where(s => s.EndDate >= now)
                .OrderBy(s => s.StartDate)
                .ToListAsync(ct);

            return Result<IEnumerable<SessionViewModel>>.Success(_mapper.Map<IEnumerable<SessionViewModel>>(sessions));
        }

        public async Task<Result<IEnumerable<MemberForSessionViewModel>>> GetSessionMembersAsync(int sessionId, bool upcoming, CancellationToken ct = default)
        {
            var session = await _uow.Repository<Session>().Query().FirstOrDefaultAsync(s => s.Id == sessionId, ct);
            if (session is null) return Result<IEnumerable<MemberForSessionViewModel>>.Failure("Session not found.");

            var now = DateTime.Now;
            var isUpcoming = session.StartDate > now;
            var isOngoing = session.StartDate <= now && session.EndDate >= now;

            if (upcoming && !isUpcoming)
                return Result<IEnumerable<MemberForSessionViewModel>>.Failure("This session is not upcoming.");
            if (!upcoming && !isOngoing)
                return Result<IEnumerable<MemberForSessionViewModel>>.Failure("This session is not ongoing.");

            var bookings = await _uow.Repository<Booking>().Query()
                .Include(b => b.Member)
                .Where(b => b.SessionId == sessionId)
                .OrderBy(b => b.Member.Name)
                .ToListAsync(ct);

            return Result<IEnumerable<MemberForSessionViewModel>>.Success(_mapper.Map<IEnumerable<MemberForSessionViewModel>>(bookings));
        }

        // Only members with an active membership can book.
        public async Task<Result<IEnumerable<MemberView>>> GetAvailableMembersAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var members = await _uow.Repository<Member>().Query()
                .Where(m => m.memberships.Any(ms => ms.EndDate > now))
                .OrderBy(m => m.Name)
                .ToListAsync(ct);

            return Result<IEnumerable<MemberView>>.Success(_mapper.Map<IEnumerable<MemberView>>(members));
        }

        public async Task<Result> CreateBookingAsync(CreateBookingViewModel model, CancellationToken ct = default)
        {
            var now = DateTime.Now;

            var session = await _uow.Repository<Session>().Query()
                .Include(s => s.bookings)
                .FirstOrDefaultAsync(s => s.Id == model.SessionId, ct);
            if (session is null) return Result.Failure("Session not found.");
            if (session.StartDate <= now) return Result.Failure("You can only book upcoming sessions.");
            if (session.bookings.Count >= session.Capacity) return Result.Failure("This session is full.");
            if (session.bookings.Any(b => b.MemberId == model.MemberId))
                return Result.Failure("This member is already booked in this session.");

            var hasActiveMembership = await _uow.Repository<Membership>().Query()
                .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > now, ct);
            if (!hasActiveMembership) return Result.Failure("This member doesn't have an active membership.");

            await _uow.Repository<Booking>().AddAsync(new Booking
            {
                MemberId = model.MemberId,
                SessionId = model.SessionId,
                BookingDate = now,
                IsAttended = false
            }, ct);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await _uow.Repository<Booking>().Query(tracking: true)
                .Include(b => b.Session)
                .FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, ct);
            if (booking is null) return Result.Failure("Booking not found.");
            if (booking.Session.StartDate <= DateTime.Now)
                return Result.Failure("You can only cancel bookings for upcoming sessions.");

            _uow.Repository<Booking>().Delete(booking);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result> MarkAsAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await _uow.Repository<Booking>().Query(tracking: true)
                .Include(b => b.Session)
                .FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, ct);
            if (booking is null) return Result.Failure("Booking not found.");

            var now = DateTime.Now;
            if (booking.Session.StartDate > now || booking.Session.EndDate < now)
                return Result.Failure("Attendance can only be marked while the session is ongoing.");
            if (booking.IsAttended) return Result.Failure("This member is already marked as attended.");

            booking.IsAttended = true;
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
