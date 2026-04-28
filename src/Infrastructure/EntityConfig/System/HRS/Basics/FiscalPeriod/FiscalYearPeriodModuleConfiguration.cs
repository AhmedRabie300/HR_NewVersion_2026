using Domain.System.HRS.Basics.FiscalPeriod;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.FiscalPeriod
{
    public class FiscalYearPeriodModuleConfiguration : IEntityTypeConfiguration<FiscalYearPeriodModule>
    {
        public void Configure(EntityTypeBuilder<FiscalYearPeriodModule> builder)
        {
            builder.ToTable("sys_FiscalYearsPeriodsModules", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FiscalYearPeriodId)
                .IsRequired()
                .HasColumnName("FiscalYearPeriodID");

            builder.Property(x => x.ModuleId)
                .IsRequired()
                .HasColumnName("ModuleID");

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasColumnName("CompanyID");

            builder.Property(x => x.OpenDate)
                .HasColumnName("OpenDate");

            builder.Property(x => x.CloseDate)
                .HasColumnName("CloseDate");

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
            builder.HasOne(x => x.FiscalYearPeriod)
                .WithMany(x => x.Modules)
                .HasForeignKey(x => x.FiscalYearPeriodId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Module)
                .WithMany()
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.FiscalYearPeriodId)
                .HasDatabaseName("IX_FiscalYearsPeriodsModules_FiscalYearPeriodId");

            builder.HasIndex(x => x.ModuleId)
                .HasDatabaseName("IX_FiscalYearsPeriodsModules_ModuleId");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_FiscalYearsPeriodsModules_CompanyId");
        }
    }
}