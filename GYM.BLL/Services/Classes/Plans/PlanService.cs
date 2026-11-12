using AutoMapper;
using GYM.BLL.Common;
using GYM.BLL.Services.Interfaces.Plans;
using GYM.BLL.ViewModels.Plans;
using GYM.DAL.Data.Models;
using GYM.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYM.BLL.Services.Classes.Plans
{
    public class PlanService : IPlanServices
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PlanView>>> GetPlansAsync(CancellationToken ct = default)
        {
            var plans = await _uow.Repository<Plan>().Query().OrderBy(p => p.Price).ToListAsync(ct);
            return Result<IEnumerable<PlanView>>.Success(_mapper.Map<IEnumerable<PlanView>>(plans));
        }

        public async Task<Result<PlanView>> GetPlan(int id, CancellationToken ct = default)
        {
            var plan = await _uow.Repository<Plan>().GetByIdAsync(id, ct);
            return plan is null
                ? Result<PlanView>.Failure("Plan not found.")
                : Result<PlanView>.Success(_mapper.Map<PlanView>(plan));
        }

        public async Task<Result> CreatePlan(PlanView model, CancellationToken ct = default)
        {
            if (await NameExistsAsync(model.Name, excludeId: null, ct))
                return Result.Failure("A plan with this name already exists.");

            var plan = new Plan
            {
                Name = model.Name.Trim(),
                Description = model.Description.Trim(),
                DurationDays = model.DurationDays,
                Price = model.Price,
                IsActive = model.IsActive
            };

            await _uow.Repository<Plan>().AddAsync(plan, ct);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result> UpdateViewCreated(PlanView model, CancellationToken ct = default)
        {
            var plan = await _uow.Repository<Plan>().GetByIdAsync(model.Id, ct);
            if (plan is null) return Result.Failure("Plan not found.");

            if (await NameExistsAsync(model.Name, excludeId: model.Id, ct))
                return Result.Failure("A plan with this name already exists.");

            plan.Name = model.Name.Trim();
            plan.Description = model.Description.Trim();
            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;

            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result> ToggleActivationAsync(int id, CancellationToken ct = default)
        {
            var plan = await _uow.Repository<Plan>().GetByIdAsync(id, ct);
            if (plan is null) return Result.Failure("Plan not found.");

            plan.IsActive = !plan.IsActive;
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result> DeletePlan(int id, CancellationToken ct = default)
        {
            var repo = _uow.Repository<Plan>();
            var plan = await repo.GetByIdAsync(id, ct);
            if (plan is null) return Result.Failure("Plan not found.");

            var hasMemberships = await _uow.Repository<Membership>().Query().AnyAsync(m => m.PlanId == id, ct);
            if (hasMemberships)
                return Result.Failure("This plan has memberships. Deactivate it instead of deleting it.");

            repo.Delete(plan);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }

        private Task<bool> NameExistsAsync(string name, int? excludeId, CancellationToken ct)
        {
            var trimmed = name.Trim();
            return _uow.Repository<Plan>().Query()
                       .AnyAsync(p => p.Name == trimmed && (!excludeId.HasValue || p.Id != excludeId.Value), ct);
        }
    }
}
