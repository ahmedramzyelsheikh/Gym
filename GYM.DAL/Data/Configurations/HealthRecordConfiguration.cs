using GYM.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GYM.DAL.Data.Configurations
{
    public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.Property(h => h.Height).HasPrecision(5, 2);
            builder.Property(h => h.Weight).HasPrecision(5, 2);
            builder.Property(h => h.BloodType).HasMaxLength(3);
            builder.Property(h => h.Note).HasMaxLength(200);
        }
    }
}
