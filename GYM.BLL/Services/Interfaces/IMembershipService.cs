using GYM.BLL.Common;
using GYM.BLL.ViewModels;

namespace GYM.BLL.Services.Interfaces
{
    public interface IMembershipService
    {
        Task<Result<IEnumerable<MemberShipViewModel>>> GetAllActiveMembershipsAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<MemberView>>> GetAvailableMembersAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<PlanView>>> GetAvailablePlansAsync(CancellationToken ct = default);
        Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct = default);
        Task<Result> CancelMembershipAsync(int memberId, CancellationToken ct = default);
    }
}
