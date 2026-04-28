using Domain.System.HRS.Basics.GradesAndClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.GradesAndClasses
{
    public class GradeTransactionConfiguration : IEntityTypeConfiguration<GradeTransaction>
    {
        public void Configure(EntityTypeBuilder<GradeTransaction> builder)
        {
            builder.ToTable("hrs_GradesTransactions", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.GradeId)
                .IsRequired()
                .HasColumnName("GradeID");

            builder.Property(x => x.TransactionTypeId)
                .IsRequired()
                .HasColumnName("TransactionTypeID");

            builder.Property(x => x.CompanyId)
                .HasColumnName("CompanyID");

            builder.Property(x => x.MinValue)
                .HasColumnType("money")
                .HasColumnName("MinValue");

            builder.Property(x => x.MaxValue)
                .HasColumnType("money")
                .HasColumnName("MaxValue");

            builder.Property(x => x.PaidAtVacation)
                .HasColumnName("PaidAtVacation")
                .HasDefaultValue(0);

            builder.Property(x => x.OnceAtPeriod)
                .HasColumnName("OnceAtPeriod")
                .HasDefaultValue(false);

            builder.Property(x => x.IntervalId)
                .HasColumnName("IntervalID");

            builder.Property(x => x.NumberOfTickets)
                .HasColumnName("NumberOfTickets");

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
            builder.HasOne(x => x.Grade)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.GradeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.TransactionType)
                .WithMany()
                .HasForeignKey(x => x.TransactionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Interval)
                .WithMany()
                .HasForeignKey(x => x.IntervalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.GradeId)
                .HasDatabaseName("IX_GradeTransactions_GradeId");

            builder.HasIndex(x => x.TransactionTypeId)
                .HasDatabaseName("IX_GradeTransactions_TransactionTypeId");

            builder.HasIndex(x => x.IntervalId)
                .HasDatabaseName("IX_GradeTransactions_IntervalId");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_GradeTransactions_CompanyId");
        }
    }
}