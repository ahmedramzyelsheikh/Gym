using GYM.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYM.DAL.Data.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(t =>
            t.HasCheckConstraint("Ck_Capacity_range", "[Capacity] >= 1 and [Capacity] <= 25")
            );

            builder.HasOne(s => s.Trainer)
                   .WithMany(s => s.Sessions)
                    .HasForeignKey(e => e.TrainerId)
                    .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Category)
                    .WithMany(u => u.sessions)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
