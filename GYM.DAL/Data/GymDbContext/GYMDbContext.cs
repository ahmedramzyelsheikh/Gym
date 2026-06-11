using GYM.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GYM.DAL.Data.GymDbContext
{
    public class GYMDbContext : DbContext
    {
        public GYMDbContext(DbContextOptions<GYMDbContext> options) : base(options) 
        { 
        }
      
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=Gym;Trusted_Connection=True;TrustServerCertificate=True;");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GYMDbContext).Assembly);
        }
       public DbSet<Plan> Plans { get; set; } 
       public DbSet<Member> Members { get; set;  }
       public DbSet<Trainer> Trainers { get; set;  }

       public DbSet<Category> Categories { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set;  }

        public  DbSet<Session> Sessions { get; set; }

        public DbSet<Booking> Bookings  { get; set; }  
        public  DbSet<Membership> Memberships { get; set; }


    }
}
