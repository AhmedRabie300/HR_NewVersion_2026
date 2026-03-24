using Domain.System.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.MasterData
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("sys_Currencies");

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

            builder.Property(x => x.EngSymbol)
                .HasMaxLength(10)
                .HasColumnName("EngSymbol");

            builder.Property(x => x.ArbSymbol)
                .HasMaxLength(10)
                .HasColumnName("ArbSymbol");

            builder.Property(x => x.DecimalFraction)
                .IsRequired()
                .HasColumnName("DecimalFraction");

            builder.Property(x => x.DecimalEngName)
                .HasMaxLength(100)
                .HasColumnName("DecimalEngName");

            builder.Property(x => x.DecimalArbName)
                .HasMaxLength(100)
                .HasColumnName("DecimalArbName");

            builder.Property(x => x.Amount)
                .HasColumnType("money")
                .HasColumnName("Amount");

            builder.Property(x => x.NoDecimalPlaces)
                .HasColumnName("NoDecimalPlaces");

            builder.Property(x => x.CompanyId)
                .HasColumnName("CompanyID");

            builder.Property(x => x.Remarks)
                .HasMaxLength(2048)
                .HasColumnName("Remarks");

            builder.Property(x => x.RegUserId)
                .HasColumnName("RegUserID");

            builder.Property(x => x.RegComputerId)
                .HasMaxLength(50)
                .HasColumnName("RegComputerID");

            builder.Property(x => x.RegDate)
                .IsRequired()
                .HasColumnName("RegDate")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Currencies_Code");
        }
    }
}