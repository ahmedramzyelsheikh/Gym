using GYM.DAL.Data.Models;

namespace GYM.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Use Query() when you need Include / Where / projections.
        IQueryable<T> Query(bool tracking = false);

        Task<IEnumerable<T>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);

        Task AddAsync(T entity, CancellationToken ct = default);
        void Update(T entity);
        void Delete(T entity);
    }
}
