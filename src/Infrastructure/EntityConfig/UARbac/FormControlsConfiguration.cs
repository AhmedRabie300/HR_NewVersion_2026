using Domain.UARbac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.EntityConfig.UARbac
{
    public class FormControlsConfiguration : IEntityTypeConfiguration<FormControl>
    {
        public void Configure(EntityTypeBuilder<FormControl> builder)
        {
            builder.ToTable("sys_FormsControls");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.FieldName)
                .HasMaxLength(100);

            builder.Property(x => x.EngCaption)
                .HasMaxLength(100);

            builder.Property(x => x.ArbCaption)
                .HasMaxLength(100);
        }
    }
}
