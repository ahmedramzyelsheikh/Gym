using GYM.DAL.Data.Models;

namespace GYM.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
