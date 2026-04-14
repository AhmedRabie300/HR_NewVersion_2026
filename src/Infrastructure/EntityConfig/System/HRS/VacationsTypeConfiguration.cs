using Domain.System.HRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS
{
    public class VacationsTypeConfiguration : IEntityTypeConfiguration<VacationsType>
    {
        public void Configure(EntityTypeBuilder<VacationsType> builder)
        {
            builder.ToTable("hrs_VacationsTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("Code");

            builder.Property(x => x.EngName)
                .HasMaxLength(100)
                .HasColumnName("EngName");

            builder.Property(x => x.ArbName)
                .HasMaxLength(100)
                .HasColumnName("ArbName");

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(100)
                .HasColumnName("ArbName4S");

            builder.Property(x => x.IsPaid)
                .HasColumnName("IsPaid");

            builder.Property(x => x.Sex)
                .HasMaxLength(1)
                .HasColumnName("Sex");

            builder.Property(x => x.IsAnnual)
                .HasColumnName("IsAnnual");

            builder.Property(x => x.IsSickVacation)
                .HasColumnName("IsSickVacation");

            builder.Property(x => x.IsFromAnnual)
                .HasColumnName("IsFromAnnual");

            builder.Property(x => x.ForSalaryTransaction)
                .HasColumnName("ForSalaryTransaction");

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

            builder.Property(x => x.OBalanceTransactionId)
                .HasColumnName("OBalanceTransactionID");

            builder.Property(x => x.OverDueVacationId)
                .HasColumnName("OverDueVacationID");

            builder.Property(x => x.Stage1Pct)
                .HasColumnName("Stage1PCT");

            builder.Property(x => x.Stage2Pct)
                .HasColumnName("Stage2PCT");

            builder.Property(x => x.Stage3Pct)
                .HasColumnName("Stage3PCT");

            builder.Property(x => x.ForDeductionTransaction)
                .HasColumnName("ForDeductionTransaction");

            builder.Property(x => x.AffectEos)
                .HasColumnName("AffectEOS");

            builder.Property(x => x.VactionTypeCaculation)
                .HasColumnName("VactionTypeCaculation");

            builder.Property(x => x.ExceededDaysType)
                .HasColumnName("ExceededDaysType");

            builder.Property(x => x.HasPayment)
                .HasColumnName("HasPayment");

            builder.Property(x => x.RoundAnnualVacBalance)
                .HasColumnName("RoundAnnualVacBalance");

            builder.Property(x => x.Religion)
                .HasMaxLength(10)
                .HasColumnName("Religion");

            builder.Property(x => x.IsOfficial)
                .HasColumnName("IsOfficial");

            builder.Property(x => x.OverlapWithAnotherVac)
                .HasColumnName("OverlapWithAnotherVac");

            builder.Property(x => x.ConsiderAllowedDays)
                .HasColumnName("ConsiderAllowedDays");

            builder.Property(x => x.TimesNoInYear)
                .HasColumnName("TimesNoInYear");

            builder.Property(x => x.AllowedDaysNo)
                .HasColumnName("AllowedDaysNo");

            builder.Property(x => x.ExcludedFromSsRequests)
                .HasColumnName("ExcludedFromSSRequests");

            // Relationships
            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_VacationsTypes_Code");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_VacationsTypes_CompanyId");
        }
    }
}