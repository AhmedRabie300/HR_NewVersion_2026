using Domain.System.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.Search
{
    public class Sys_ObjectsConfiguration : IEntityTypeConfiguration<Sys_Objects>
    {
        public void Configure(EntityTypeBuilder<Sys_Objects> builder)
        {
            builder.ToTable("sys_Objects");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(255)
                .IsRequired()
                .HasColumnName("Code");

            builder.Property(x => x.EngName)
                .HasMaxLength(255)
                .HasColumnName("EngName");

            builder.Property(x => x.ArbName)
                .HasMaxLength(255)
                .HasColumnName("ArbName");

            builder.Property(x => x.IsFiscalYearClosable)
                .HasColumnName("IsFiscalYearClosable");

            builder.Property(x => x.RegDate)
                .IsRequired()
                .HasColumnName("RegDate")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Sys_Objects_Code");
        }
    }
}