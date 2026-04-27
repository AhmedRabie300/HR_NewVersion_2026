using Domain.System.HRS.Basics.GradesAndClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.GradesAndClasses
{
    public class GradeStepTransactionConfiguration : IEntityTypeConfiguration<GradeStepTransaction>
    {
        public void Configure(EntityTypeBuilder<GradeStepTransaction> builder)
        {
            builder.ToTable("hrs_GradesStepsTransactions", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.GradeStepId)
                .IsRequired()
                .HasColumnName("GradeStepID");

            builder.Property(x => x.GradeTransactionId)
                .HasColumnName("GradeTransactionID");

            builder.Property(x => x.CompanyId)
                .HasColumnName("CompanyID");

            builder.Property(x => x.Amount)
                .HasColumnType("money")
                .HasColumnName("Amount");

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

            builder.Property(x => x.Active)
                .HasColumnName("Active");

            builder.Property(x => x.ActiveDate)
                .HasColumnName("ActiveDate");

            builder.Property(x => x.ActiveDateD)
                .HasMaxLength(3)
                .HasColumnName("ActiveDate_D");

            // Relationships
            builder.HasOne(x => x.GradeStep)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.GradeStepId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.GradeTransaction)
                .WithMany()
                .HasForeignKey(x => x.GradeTransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.GradeStepId)
                .HasDatabaseName("IX_GradeStepTransactions_GradeStepId");

            builder.HasIndex(x => x.GradeTransactionId)
                .HasDatabaseName("IX_GradeStepTransactions_GradeTransactionId");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_GradeStepTransactions_CompanyId");
        }
    }
}