using GYM.BLL.Common;
using GYM.BLL.ViewModels;

namespace GYM.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Result<IEnumerable<SessionViewModel>>> GetManageableSessionsAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<MemberForSessionViewModel>>> GetSessionMembersAsync(int sessionId, bool upcoming, CancellationToken ct = default);
        Task<Result<IEnumerable<MemberView>>> GetAvailableMembersAsync(CancellationToken ct = default);
        Task<Result> CreateBookingAsync(CreateBookingViewModel model, CancellationToken ct = default);
        Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default);
        Task<Result> MarkAsAttendedAsync(int memberId, int sessionId, CancellationToken ct = default);
    }
}
