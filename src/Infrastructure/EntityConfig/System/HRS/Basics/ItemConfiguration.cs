using Domain.System.HRS.Basics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("hrs_Items", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("Code");

            builder.Property(x => x.EngName)
                .HasMaxLength(500)
                .HasColumnName("EngName");

            builder.Property(x => x.ArbName)
                .HasMaxLength(500)
                .HasColumnName("ArbName");

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(500)
                .HasColumnName("ArbName4S");

            builder.Property(x => x.PurchaseDate)
                .HasColumnName("PurchaseDate");

            builder.Property(x => x.PurchasePrice)
                .HasColumnType("money")
                .HasColumnName("PurchasePrice");

            builder.Property(x => x.ExpiryDate)
                .HasColumnName("ExpiryDate");

            builder.Property(x => x.LicenseNumber)
                .HasMaxLength(100)
                .HasColumnName("LicenseNumber");

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasColumnName("CompanyID");

            builder.Property(x => x.IsFromAssets)
                .HasColumnName("IsFromAssets");

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
            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Items_Code");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_Items_CompanyId");

            builder.HasIndex(x => x.EngName)
                .HasDatabaseName("IX_Items_EngName");

            builder.HasIndex(x => x.ArbName)
                .HasDatabaseName("IX_Items_ArbName");
        }
    }
}