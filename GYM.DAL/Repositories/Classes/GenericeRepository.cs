using GYM.DAL.Data.GymDbContext;
using GYM.DAL.Data.Models;
using GYM.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYM.DAL.Repositories.Classes
{
    public class GenericeRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly GYMDbContext _context;
        private readonly DbSet<T> _set;

        public GenericeRepository(GYMDbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public IQueryable<T> Query(bool tracking = false)
            => tracking ? _set : _set.AsNoTracking();

        public async Task<IEnumerable<T>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
            => await Query(tracking).ToListAsync(ct);

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _set.FirstOrDefaultAsync(e => e.Id == id, ct);

        public async Task AddAsync(T entity, CancellationToken ct = default)
            => await _set.AddAsync(entity, ct);

        public void Update(T entity) => _set.Update(entity);

        public void Delete(T entity) => _set.Remove(entity);
    }
}
