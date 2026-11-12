using AutoMapper;
using GYM.BLL.Common;
using GYM.BLL.Services.Interfaces;
using GYM.BLL.ViewModels;
using GYM.DAL.Data.Models;
using GYM.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYM.BLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MemberShipViewModel>>> GetAllActiveMembershipsAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var memberships = await _uow.Repository<Membership>().Query()
                .Include(m => m.Member)
                .Include(m => m.Plan)
                .Where(m => m.EndDate > now)
                .OrderBy(m => m.EndDate)
                .ToListAsync(ct);

            return Result<IEnumerable<MemberShipViewModel>>.Success(_mapper.Map<IEnumerable<MemberShipViewModel>>(memberships));
        }

        // Members who don't have an active membership yet.
        public async Task<Result<IEnumerable<MemberView>>> GetAvailableMembersAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var members = await _uow.Repository<Member>().Query()
                .Where(m => !m.memberships.Any(ms => ms.EndDate > now))
                .OrderBy(m => m.Name)
                .ToListAsync(ct);

            return Result<IEnumerable<MemberView>>.Success(_mapper.Map<IEnumerable<MemberView>>(members));
        }

        public async Task<Result<IEnumerable<PlanView>>> GetAvailablePlansAsync(CancellationToken ct = default)
        {
            var plans = await _uow.Repository<Plan>().Query()
                .Where(p => p.IsActive)
                .OrderBy(p => p.Price)
                .ToListAsync(ct);

            return Result<IEnumerable<PlanView>>.Success(_mapper.Map<IEnumerable<PlanView>>(plans));
        }

        public async Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct = default)
        {
            var now = DateTime.Now;

            if (!await _uow.Repository<Member>().Query().AnyAsync(m => m.Id == model.MemberId, ct))
                return Result.Failure("Member not found.");

            var plan = await _uow.Repository<Plan>().Query().FirstOrDefaultAsync(p => p.Id == model.PlanId, ct);
            if (plan is null) return Result.Failure("Plan not found.");
            if (!plan.IsActive) return Result.Failure("This plan is not active.");

            var hasActive = await _uow.Repository<Membership>().Query()
                .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > now, ct);
            if (hasActive) return Result.Failure("This member already has an active membership.");

            var membership = new Membership
            {
                MemberId = model.MemberId,
                PlanId = model.PlanId,
                CreateAt = now,
                EndDate = now.AddDays(plan.DurationDays)
            };

            await _uow.Repository<Membership>().AddAsync(membership, ct);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }

        // Ends the member's active membership now (history is kept).
        public async Task<Result> CancelMembershipAsync(int memberId, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var membership = await _uow.Repository<Membership>().Query(tracking: true)
                .FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > now, ct);
            if (membership is null) return Result.Failure("No active membership found for this member.");

            membership.EndDate = now;
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
