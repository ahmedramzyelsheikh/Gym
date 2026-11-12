using GYM.BLL.Common;
using GYM.BLL.Services.Interfaces.Analytics;
using GYM.BLL.ViewModels.Analytics;
using GYM.DAL.Data.Models;
using GYM.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYM.BLL.Services.Classes.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _uow;

        public AnalyticsService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<AnalyticsViewModel>> GetDashboardAnalyticsAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var sessions = _uow.Repository<Session>().Query();

            var model = new AnalyticsViewModel
            {
                TotalMembers = await _uow.Repository<Member>().Query().CountAsync(ct),
                ActiveMembers = await _uow.Repository<Membership>().Query()
                                    .Where(m => m.EndDate > now)
                                    .Select(m => m.MemberId).Distinct().CountAsync(ct),
                TotalTrainers = await _uow.Repository<Trainer>().Query().CountAsync(ct),
                UpcomingSessions = await sessions.CountAsync(s => s.StartDate > now, ct),
                OngoingSessions = await sessions.CountAsync(s => s.StartDate <= now && s.EndDate >= now, ct),
                CompletedSessions = await sessions.CountAsync(s => s.EndDate < now, ct)
            };

            return Result<AnalyticsViewModel>.Success(model);
        }
    }
}
