using Domain.System.HRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.ToTable("Hrs_Gender");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .HasColumnName("Code");

            builder.Property(x => x.EngName)
                .HasMaxLength(50)
                .HasColumnName("EngName");

            builder.Property(x => x.ArbName)
                .HasMaxLength(50)
                .HasColumnName("ArbName");

            builder.Property(x => x.RegUserId)
                .HasColumnName("RegUserID");

            builder.Property(x => x.RegDate)
                .HasColumnName("RegDate");

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Gender_Code")
                .HasFilter("[Code] IS NOT NULL");
        }
    }
}