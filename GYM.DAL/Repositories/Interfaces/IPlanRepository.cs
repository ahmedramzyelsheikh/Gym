using GYM.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYM.BLL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
         Task<IEnumerable<Plan>> GetAllAync(bool tracking = false, CancellationToken ct = default);
         Task<Plan?> GetById(int id, CancellationToken et = default);
        void Add(Plan plan, CancellationToken et = default);
        void Update(Plan plan, CancellationToken et = default   );
        void Delete(Plan plan, CancellationToken et = default);
        int SaveChanges(CancellationToken et = default);
    }
}
