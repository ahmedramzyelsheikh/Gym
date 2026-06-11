using GYM.BLL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using GYM.DAL.Data.GymDbContext;
using GYM.DAL.Data.Models;

namespace GYM.BLL.Repositories.ImplementionsRepositoies
{
    public class planRepository : IPlanRepository
    {
        private readonly GYMDbContext _context;

        public planRepository(GYMDbContext _context)
        {
            this._context = _context;
        }

        public void Add(Plan plan,  CancellationToken et = default)
        {
            _context.Plans.Add(plan); 
            _context.SaveChanges();
        }

        public void Delete(Plan plan , CancellationToken et = default)
        {
            _context.Plans.Remove(plan);
            _context.SaveChanges();
        }

        public async Task<Plan?> GetById(int id , CancellationToken et = default)
        {
              var  result  =  await _context.Plans.FindAsync(id);
            if(result == null)
            {
                return null;
            }
            return result;
        }

        public int SaveChanges(CancellationToken et = default)
        {
           return  _context.SaveChanges();
        }

        public void Update(Plan plan , CancellationToken et = default)
        {
            throw new NotImplementedException();
        }

        public  async Task<IEnumerable<Plan>> GetAllAync(bool  tracking =  false  ,CancellationToken ct =  default)
        {
            var result =   tracking? _context.Plans.ToListAsync(ct) : _context.Plans.AsNoTracking().ToListAsync(ct);

            return  await result;
        }

      
    }
}
