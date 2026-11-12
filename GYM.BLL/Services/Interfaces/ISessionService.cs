using GYM.BLL.Common;
using GYM.BLL.ViewModels;

namespace GYM.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<Result<IEnumerable<SessionViewModel>>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<Result<SessionViewModel>> GetSessionDetailsAsync(int id, CancellationToken ct = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
        Task<Result<UpdateSessionViewModel>> GetSessionToEditAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(UpdateSessionViewModel model, CancellationToken ct = default);
        Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default);
        Task<Result<IEnumerable<CategoryViewModel>>> GetCategoriesAsync(CancellationToken ct = default);
        Task<Result<IEnumerable<TrainerView>>> GetTrainersAsync(CancellationToken ct = default);
    }
}
