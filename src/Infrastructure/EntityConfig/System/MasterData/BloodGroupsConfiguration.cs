using Domain.System.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.EntityConfig.System.MasterData
{
    public class BloodGroupsConfiguration : IEntityTypeConfiguration<BloodGroups>
    {
        public void Configure(EntityTypeBuilder<BloodGroups> builder)
        {

            builder.ToTable("hrs_BloodGroups");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

        }
    }
}
