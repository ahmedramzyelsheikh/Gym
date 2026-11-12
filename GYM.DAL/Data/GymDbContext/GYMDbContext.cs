using GYM.DAL.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GYM.DAL.Data.GymDbContext
{
    public class GYMDbContext : IdentityDbContext<AppUser>
    {
        public GYMDbContext(DbContextOptions<GYMDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GYMDbContext).Assembly);
        }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Membership> Memberships { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            ApplyAuditFields();
            return base.SaveChanges();
        }

        // Fills CreateAt / UpdateAt on every BaseEntity automatically.
        private void ApplyAuditFields()
        {
            var now = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity.CreateAt == default) entry.Entity.CreateAt = now;
                    entry.Entity.UpdateAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdateAt = now;
                }
            }
        }
    }
}
