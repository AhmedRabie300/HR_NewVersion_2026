using Domain.System.HRS.Basics.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.Contracts
{
    public class ContractTransactionConfiguration : IEntityTypeConfiguration<ContractTransaction>
    {
        public void Configure(EntityTypeBuilder<ContractTransaction> builder)
        {
            builder.ToTable("hrs_ContractsTransactions", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ContractId)
                .IsRequired()
                .HasColumnName("ContractID");

            builder.Property(x => x.TransactionTypeId)
                .IsRequired()
                .HasColumnName("TransactionTypeID");

            builder.Property(x => x.Amount)
                .HasColumnType("money")
                .HasColumnName("Amount");

            builder.Property(x => x.Active)
                .HasColumnName("Active");

            builder.Property(x => x.IntervalId)
                .HasColumnName("IntervalID");

            builder.Property(x => x.PaidAtVacation)
                .HasColumnName("PaidAtVacation");

            builder.Property(x => x.OnceAtPeriod)
                .HasColumnName("OnceAtPeriod");

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

            builder.Property(x => x.ActiveDate)
                .HasColumnName("ActiveDate");

            builder.Property(x => x.ActiveDateD)
                .HasMaxLength(3)
                .HasColumnName("ActiveDate_D");

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasColumnName("CompanyID");

            // Relationships
            builder.HasOne(x => x.Contract)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.ContractId)
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
            builder.HasIndex(x => x.ContractId)
                .HasDatabaseName("IX_ContractTransactions_ContractId");

            builder.HasIndex(x => x.TransactionTypeId)
                .HasDatabaseName("IX_ContractTransactions_TransactionTypeId");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_ContractTransactions_CompanyId");
        }
    }
}