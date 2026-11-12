using GYM.BLL.Common;
using GYM.BLL.ViewModels.Analytics;

namespace GYM.BLL.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<Result<AnalyticsViewModel>> GetDashboardAnalyticsAsync(CancellationToken ct = default);
    }
}
