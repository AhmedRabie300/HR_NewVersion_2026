using Domain.System.HRS.Basics.HICompanies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.HICompanies
{
    public class HICompanyClassConfiguration : IEntityTypeConfiguration<HICompanyClass>
    {
        public void Configure(EntityTypeBuilder<HICompanyClass> builder)
        {
            builder.ToTable("hrs_HICompanyClasses", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.HICompanyId)
                .IsRequired()
                .HasColumnName("HICompanyID");

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasColumnName("CompanyID");

            builder.Property(x => x.EngName)
                .HasMaxLength(50)
                .HasColumnName("EngName");

            builder.Property(x => x.ArbName)
                .HasMaxLength(50)
                .HasColumnName("ArbName");

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(50)
                .HasColumnName("ArbName4S");

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

            builder.Property(x => x.CompanyAmount)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("CompanyAmount");

            builder.Property(x => x.EmployeeAmount)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("EmployeeAmount");

            // Relationships
            builder.HasOne(x => x.HICompany)
                .WithMany(x => x.Classes)
                .HasForeignKey(x => x.HICompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.HICompanyId)
                .HasDatabaseName("IX_HICompanyClasses_HICompanyId");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_HICompanyClasses_CompanyId");
        }
    }
}