using GYM.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYM.DAL.Data.Configurations
{
    public class GymUserConfiguration : IEntityTypeConfiguration<GymUser>
    {
        public void Configure(EntityTypeBuilder<GymUser> builder)
        {

            builder.Property(p => p.Name)
                   .HasMaxLength(50);



            builder.Property(e => e.Email)
                   .HasMaxLength(100);

            builder.OwnsOne(e => e.Address, addressBuilder =>
            {
               
                addressBuilder.Property(a => a.City)
                    .HasMaxLength(30);

                addressBuilder.Property(a => a.Street)
                    .HasMaxLength(30);
              
            });

            builder.HasIndex(e => e.Phone)
                   .IsUnique();

            builder.Property(e => e.Email)
                   .HasMaxLength(100);

            builder.HasIndex(e => e.Email)
                   .IsUnique();

            builder.Property(e => e.CreateAt)
                  .HasDefaultValueSql("GETDATE()");
            builder.ToTable(t =>

              t.HasCheckConstraint("CK_GymUser_ValidEgyptianPhone",
              "[Phone] LIKE '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'")
             );
            builder.ToTable(t =>
                 t.HasCheckConstraint("Ck_GymUser_validemail", "[Email] LIKE '%_@__%.__%'")
                );
        }
    }
}
