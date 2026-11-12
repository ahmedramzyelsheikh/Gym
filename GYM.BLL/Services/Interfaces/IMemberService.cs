using GYM.BLL.Common;
using GYM.BLL.ViewModels;

namespace GYM.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<Result<IEnumerable<MemberView>>> GetAllMembersAsync(CancellationToken ct = default);
        Task<Result<MemberView>> GetMemberAsync(int id, CancellationToken ct = default);
        Task<Result> CreateMemberAsynce(CreateMemberViewModel model, CancellationToken ct = default);
        Task<Result<HealthRecordViewModel>> GetDetailsHealthRecord(int memberId, CancellationToken ct = default);
        Task<Result<MemberToUpdateViewModel>> GetMemberToEditAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateMemberAsync(MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<Result> DeleteMemberAsync(int id, CancellationToken ct = default);
    }
}
