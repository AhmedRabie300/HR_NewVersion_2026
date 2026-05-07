using Domain.System.HRS.Basics.EmployeesItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.EmployeesItems
{
    public class EmployeeItemConfiguration : IEntityTypeConfiguration<EmployeeItem>
    {
        public void Configure(EntityTypeBuilder<EmployeeItem> builder)
        {
            builder.ToTable("hrs_EmployeesItems", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EmployeeId)
                .IsRequired()
                .HasColumnName("EmployeeID");

            builder.Property(x => x.ItemId)
                .IsRequired()
                .HasColumnName("ItemID");

            builder.Property(x => x.ReceivedDate)
                .HasColumnName("ReceivedDate");

            builder.Property(x => x.ReturnedDate)
                .HasColumnName("ReturnedDate");

            builder.Property(x => x.ReceivingItemStatus)
                .HasMaxLength(100)
                .HasColumnName("ReceivingItemstatus");

            builder.Property(x => x.ReturningItemStatus)
                .HasMaxLength(100)
                .HasColumnName("ReturningItemstatus");

            builder.Property(x => x.IsFromAssets)
                .HasColumnName("IsFromAssets");

            builder.Property(x => x.IsConfirmed)
                .HasColumnName("IsConfirmed");

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

            builder.Property(x => x.CompanyId)
                .HasColumnName("CompanyID");

            // Relationships
            builder.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Item)
                .WithMany()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.EmployeeId)
                .HasDatabaseName("IX_EmployeesItems_EmployeeId");

            builder.HasIndex(x => x.ItemId)
                .HasDatabaseName("IX_EmployeesItems_ItemId");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_EmployeesItems_CompanyId");

            builder.HasIndex(x => x.IsConfirmed)
                .HasDatabaseName("IX_EmployeesItems_IsConfirmed");

            builder.HasIndex(x => new { x.EmployeeId, x.IsConfirmed })
                .HasDatabaseName("IX_EmployeesItems_Employee_Confirmed");
        }
    }
}