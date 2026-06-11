using GYM.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYM.DAL.Data.Configurations
{
    public class MemberShipConfiguartion : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.Property(m => m.CreateAt)
                 .HasColumnName("Start Date")
                 .HasDefaultValueSql("GETDATE()");
        }
    }
}
