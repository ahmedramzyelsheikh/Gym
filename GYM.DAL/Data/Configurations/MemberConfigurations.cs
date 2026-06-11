using GYM.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYM.DAL.Data.Configurations
{
    internal class MemberConfigurations : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            

            builder.Property(e => e.CreateAt)
                    .HasColumnName("JoinDate");

           

        }
    }
}
