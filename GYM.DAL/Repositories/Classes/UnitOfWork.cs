using GYM.DAL.Data.GymDbContext;
using GYM.DAL.Data.Models;
using GYM.DAL.Repositories.Interfaces;

namespace GYM.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GYMDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(GYMDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            if (!_repositories.TryGetValue(typeof(T), out var repository))
            {
                repository = new GenericeRepository<T>(_context);
                _repositories[typeof(T)] = repository;
            }
            return (IGenericRepository<T>)repository;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);
    }
}
