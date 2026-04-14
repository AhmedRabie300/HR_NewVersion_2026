using Domain.System.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.MasterData
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("sys_Countries");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("Code");

            builder.Property(x => x.EngName)
                .HasMaxLength(255)
                .HasColumnName("EngName");

            builder.Property(x => x.ArbName)
                .HasMaxLength(255)
                .HasColumnName("ArbName");

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(255)
                .HasColumnName("ArbName4S");

            builder.Property(x => x.CurrencyId)
                .HasColumnName("CurrencyID");

            builder.Property(x => x.NationalityId)
                .HasColumnName("NationalityID");

            builder.Property(x => x.PhoneKey)
                .HasMaxLength(50)
                .HasColumnName("PhoneKey");

            builder.Property(x => x.IsMainCountries)
                .HasColumnName("IsMainCountries");

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

            builder.Property(x => x.RegionId)
                .HasColumnName("RegionID");

            builder.Property(x => x.ISOAlpha2)
                .HasMaxLength(2)
                .HasColumnName("ISOAlpha2");

            builder.Property(x => x.ISOAlpha3)
                .HasMaxLength(3)
                .HasColumnName("ISOAlpha3");

            builder.Property(x => x.Languages)
                .HasMaxLength(100)
                .HasColumnName("Languages");

            builder.Property(x => x.Continent)
                .HasMaxLength(10)
                .HasColumnName("Continent");

            builder.Property(x => x.CapitalId)
                .HasColumnName("CapitalID");

            // Relationships
            builder.HasOne(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Nationality)
                .WithMany()
                .HasForeignKey(x => x.NationalityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Region)
                .WithMany()
                .HasForeignKey(x => x.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Capital)
                .WithMany()
                .HasForeignKey(x => x.CapitalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Countries_Code");

            builder.HasIndex(x => x.CurrencyId)
                .HasDatabaseName("IX_Countries_CurrencyId");

            builder.HasIndex(x => x.NationalityId)
                .HasDatabaseName("IX_Countries_NationalityId");

            builder.HasIndex(x => x.RegionId)
                .HasDatabaseName("IX_Countries_RegionId");

            builder.HasIndex(x => x.ISOAlpha2)
                .HasDatabaseName("IX_Countries_ISOAlpha2");

            builder.HasIndex(x => x.ISOAlpha3)
                .HasDatabaseName("IX_Countries_ISOAlpha3");
        }
    }
}