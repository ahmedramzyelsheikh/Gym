using GYM.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GYM.DAL.Data.Configurations
{
    public class PlanConfiguration :IEntityTypeConfiguration<Plan>
    {

       
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(50);

            builder.Property(p => p.Description)
                   .HasMaxLength(100);

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)")
                   .HasPrecision(10, 2);

            builder.Property(p => p.CreateAt)
                   .HasDefaultValueSql("GETDATE()");


            builder.ToTable(t =>
            {
                t.HasCheckConstraint("PlanDurationCheck", "DurationDays Between 1 and 365");
            });

            // Plans are seeded at startup from wwwroot/Files/plans.json (see DataSeeder).
        }
    }
}
