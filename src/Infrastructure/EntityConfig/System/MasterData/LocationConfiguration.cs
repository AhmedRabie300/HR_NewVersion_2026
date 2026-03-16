// Infrastructure/EntityConfig/System/MasterData/LocationConfiguration.cs
using Domain.System.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.MasterData
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("sys_Locations");

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

            builder.Property(x => x.CityId)
                .HasColumnName("CityID");

            builder.Property(x => x.BranchId)
                .HasColumnName("BranchID");

            builder.Property(x => x.StoreId)
                .HasColumnName("StoreID");

            builder.Property(x => x.InventoryCostLedgerId)
                .HasColumnName("InventoryCostLedgerID");

            builder.Property(x => x.InventoryAdjustmentLedgerId)
                .HasColumnName("InventoryAdjustmentLedgerID");

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

            builder.Property(x => x.DepartmentId)
                .HasColumnName("DepartmentID");

            builder.Property(x => x.CostCenterCode1)
                .HasMaxLength(50)
                .HasColumnName("CostCenterCode1");

            builder.Property(x => x.CostCenterCode2)
                .HasMaxLength(50)
                .HasColumnName("CostCenterCode2");

            builder.Property(x => x.CostCenterCode3)
                .HasMaxLength(50)
                .HasColumnName("CostCenterCode3");

            builder.Property(x => x.CostCenterCode4)
                .HasMaxLength(50)
                .HasColumnName("CostCenterCode4");

            // Relationships
            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Locations_Code");

            builder.HasIndex(x => x.EngName)
                .HasDatabaseName("IX_Locations_EngName");

            builder.HasIndex(x => x.ArbName)
                .HasDatabaseName("IX_Locations_ArbName");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_Locations_CompanyId");

            builder.HasIndex(x => x.BranchId)
                .HasDatabaseName("IX_Locations_BranchId");

            builder.HasIndex(x => x.DepartmentId)
                .HasDatabaseName("IX_Locations_DepartmentId");

            builder.HasIndex(x => x.CityId)
                .HasDatabaseName("IX_Locations_CityId");

            builder.HasIndex(x => x.StoreId)
                .HasDatabaseName("IX_Locations_StoreId");
        }
    }
}