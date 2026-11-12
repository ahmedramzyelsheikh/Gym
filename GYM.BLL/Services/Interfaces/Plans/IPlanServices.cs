using GYM.BLL.Common;
using GYM.BLL.ViewModels.Plans;

namespace GYM.BLL.Services.Interfaces.Plans
{
    public interface IPlanServices
    {
        Task<Result<IEnumerable<PlanView>>> GetPlansAsync(CancellationToken ct = default);
        Task<Result<PlanView>> GetPlan(int id, CancellationToken ct = default);
        Task<Result> CreatePlan(PlanView model, CancellationToken ct = default);
        Task<Result> UpdateViewCreated(PlanView model, CancellationToken ct = default);
        Task<Result> ToggleActivationAsync(int id, CancellationToken ct = default);
        Task<Result> DeletePlan(int id, CancellationToken ct = default);
    }
}
