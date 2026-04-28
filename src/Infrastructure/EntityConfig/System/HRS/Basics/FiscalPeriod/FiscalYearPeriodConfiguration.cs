using Domain.System.HRS.Basics.FiscalPeriod;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.FiscalPeriod
{
    public class FiscalYearPeriodConfiguration : IEntityTypeConfiguration<FiscalYearPeriod>
    {
        public void Configure(EntityTypeBuilder<FiscalYearPeriod> builder)
        {
            builder.ToTable("sys_FiscalYearsPeriods", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(30)
                .HasColumnName("Code");

            builder.Property(x => x.FiscalYearId)
                .IsRequired()
                .HasColumnName("FiscalYearID");

            builder.Property(x => x.EngName)
                .HasMaxLength(100)
                .HasColumnName("EngName");

            builder.Property(x => x.ArbName)
                .HasMaxLength(100)
                .HasColumnName("ArbName");

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(100)
                .HasColumnName("ArbName4S");

            builder.Property(x => x.FromDate)
                .HasColumnName("FromDate");

            builder.Property(x => x.ToDate)
                .HasColumnName("ToDate");

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

            builder.Property(x => x.HFromDate)
                .HasMaxLength(12)
                .HasColumnName("HFromDate");

            builder.Property(x => x.HToDate)
                .HasMaxLength(12)
                .HasColumnName("HToDate");

            builder.Property(x => x.PeriodType)
                .HasColumnName("PeriodType");

            builder.Property(x => x.PeriodRank)
                .HasColumnName("PeriodRank");

            builder.Property(x => x.PrepareFromDate)
                .HasColumnName("PrepareFromDate");

            builder.Property(x => x.PrepareToDate)
                .HasColumnName("PrepareToDate");

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasColumnName("CompanyID");

            // Relationships
            builder.HasOne(x => x.FiscalYear)
                .WithMany()
                .HasForeignKey(x => x.FiscalYearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.FiscalYearId)
                .HasDatabaseName("IX_FiscalYearsPeriods_FiscalYearId");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_FiscalYearsPeriods_CompanyId");

            builder.HasIndex(x => x.PeriodRank)
                .HasDatabaseName("IX_FiscalYearsPeriods_PeriodRank");
        }
    }
}