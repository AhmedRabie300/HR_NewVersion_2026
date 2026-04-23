using Domain.System.HRS.Basics.HICompanies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.HICompanies
{
    public class HICompanyConfiguration : IEntityTypeConfiguration<HICompany>
    {
        public void Configure(EntityTypeBuilder<HICompany> builder)
        {
            builder.ToTable("hrs_HICompanies", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("Code");

            builder.Property(x => x.EngName)
                .HasMaxLength(50)
                .HasColumnName("EngName");

            builder.Property(x => x.ArbName)
                .HasMaxLength(50)
                .HasColumnName("ArbName");

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(50)
                .HasColumnName("ArbName4S");

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasColumnName("CompanyID");

            builder.Property(x => x.Remarks)
                .HasMaxLength(2048)
                .HasColumnName("Remarks");

            builder.Property(x => x.RegUserId)
                .HasColumnName("RegUserID");

            builder.Property(x => x.RegComputerId)
                .HasColumnName("RegComputerID");

            builder.Property(x => x.RegDate)
                .IsRequired()
                .HasColumnName("RegDate")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

            // Relationships
            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_HICompanies_Code");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_HICompanies_CompanyId");
        }
    }
}