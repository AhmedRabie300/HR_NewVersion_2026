using Domain.System.HRS.VacationAndEndOfService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.VacationAndEndOfService
{
    public class EndOfServiceRuleConfiguration : IEntityTypeConfiguration<EndOfServiceRule>
    {
        public void Configure(EntityTypeBuilder<EndOfServiceRule> builder)
        {
            builder.ToTable("hrs_EndOfServicesRules", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EndOfServiceId)
                .IsRequired()
                .HasColumnName("EndOfServiceID");

            builder.Property(x => x.FromWorkingMonths)
                .HasColumnName("FromWorkingMonths");

            builder.Property(x => x.ToWorkingMonths)
                .HasColumnName("ToWorkingMonths");

            builder.Property(x => x.AmountPercent)
                .HasColumnName("AmountPercent");

            builder.Property(x => x.Formula)
                .HasMaxLength(512)
                .HasColumnName("Formula");

            builder.Property(x => x.ExtraDedFormula)
                .HasMaxLength(512)
                .HasColumnName("ExtraDedFormula");

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
            builder.HasOne(x => x.EndOfService)
                .WithMany(x => x.Rules)
                .HasForeignKey(x => x.EndOfServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.EndOfServiceId)
                .HasDatabaseName("IX_EndOfServicesRules_EndOfServiceId");
        }
    }
}