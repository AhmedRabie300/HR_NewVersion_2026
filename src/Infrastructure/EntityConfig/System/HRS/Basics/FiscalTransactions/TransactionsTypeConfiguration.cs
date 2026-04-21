using Domain.System.HRS.Basics.FiscalTransactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.FiscalTransactions
{
    public class TransactionsTypeConfiguration : IEntityTypeConfiguration<TransactionsType>
    {
        public void Configure(EntityTypeBuilder<TransactionsType> builder)
        {
            builder.ToTable("hrs_TransactionsTypes", tb => tb.UseSqlOutputClause(false));

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

            builder.Property(x => x.ShortEngName)
                .HasMaxLength(50)
                .HasColumnName("ShortEngName");

            builder.Property(x => x.ShortArbName)
                .HasMaxLength(50)
                .HasColumnName("ShortArbName");

            builder.Property(x => x.ShortArbName4S)
                .HasMaxLength(50)
                .HasColumnName("ShortArbName4S");

            builder.Property(x => x.TransactionGroupId)
                .IsRequired()
                .HasColumnName("TransactionGroupID");

            builder.Property(x => x.Sign)
                .IsRequired()
                .HasColumnName("Sign");

            builder.Property(x => x.DebitAccountCode)
                .HasMaxLength(50)
                .HasColumnName("DebitAccountCode");

            builder.Property(x => x.CreditAccountCode)
                .HasMaxLength(50)
                .HasColumnName("CreditAccountCode");

            builder.Property(x => x.IsPaid)
                .HasColumnName("IsPaid");

            builder.Property(x => x.Formula)
                .HasMaxLength(2048)
                .HasColumnName("Formula");

            builder.Property(x => x.BeginContractFormula)
                .HasMaxLength(2048)
                .HasColumnName("BeginContractFormula");

            builder.Property(x => x.EndContractFormula)
                .HasMaxLength(2048)
                .HasColumnName("EndContractFormula");

            builder.Property(x => x.InputIsNumeric)
                .HasColumnName("InputIsNumeric");

            builder.Property(x => x.IsEndOfService)
                .HasColumnName("IsEndOfService");

            builder.Property(x => x.IsSalaryEOSExeclude)
                .HasColumnName("IsSalaryEOSExeclude");

            builder.Property(x => x.IsProjectRelatedItem)
                .HasColumnName("IsProjectRelatedItem");

            builder.Property(x => x.IsBasicSalary)
                .HasColumnName("IsBasicSalary");

            builder.Property(x => x.IsDistributable)
                .HasColumnName("IsDistributable");

            builder.Property(x => x.IsAllowPosting)
                .HasColumnName("IsAllowPosting");

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasColumnName("CompanyID");

            builder.Property(x => x.Remarks)
                .HasMaxLength(2048)
                .HasColumnName("Remarks");

            builder.Property(x => x.RegUserId)
                .IsRequired()
                .HasColumnName("RegUserID");

            builder.Property(x => x.RegComputerId)
                .HasColumnName("RegComputerID");

            builder.Property(x => x.RegDate)
                .IsRequired()
                .HasColumnName("RegDate")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

            builder.Property(x => x.HasInsuranceTiers)
                .HasColumnName("HasInsuranceTiers");

             builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TransactionGroup)
                .WithMany()
                .HasForeignKey(x => x.TransactionGroupId)
                .OnDelete(DeleteBehavior.Restrict);

             builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_TransactionsTypes_Code");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_TransactionsTypes_CompanyId");

            builder.HasIndex(x => x.TransactionGroupId)
                .HasDatabaseName("IX_TransactionsTypes_TransactionGroupId");
        }
    }
}