
using Domain.System.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.MasterData
{
    public class SectorConfiguration : IEntityTypeConfiguration<Sector>
    {
        public void Configure(EntityTypeBuilder<Sector> builder)
        {
            builder.ToTable("sys_Sectors");

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

            builder.Property(x => x.ParentId)
                .HasColumnName("ParentID");

            builder.Property(x => x.CompanyId)
                .IsRequired()
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

            // Relationships
            builder.HasOne(x => x.Company)
                .WithMany(x => x.Sectors)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ParentSector)
                .WithMany(x => x.ChildSectors)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Sectors_Code");

            builder.HasIndex(x => x.EngName)
                .HasDatabaseName("IX_Sectors_EngName");

            builder.HasIndex(x => x.ArbName)
                .HasDatabaseName("IX_Sectors_ArbName");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_Sectors_CompanyId");

            builder.HasIndex(x => x.ParentId)
                .HasDatabaseName("IX_Sectors_ParentId");
        }
    }
}