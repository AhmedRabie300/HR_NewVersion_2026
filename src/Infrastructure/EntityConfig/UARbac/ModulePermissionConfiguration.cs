 using Domain.UARbac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.UARbac
{
    public class ModulePermissionConfiguration : IEntityTypeConfiguration<ModulePermission>
    {
        public void Configure(EntityTypeBuilder<ModulePermission> builder)
        {
             builder.ToTable("sys_ModulesPermissions");

             builder.HasKey(x => x.Id);

             builder.Property(x => x.ModuleId)
                .IsRequired()
                .HasColumnName("ModuleID");

            builder.Property(x => x.GroupId)
                .HasColumnName("GroupID");

            builder.Property(x => x.UserId)
                .HasColumnName("UserID");

            builder.Property(x => x.CanView)
                .HasColumnName("CanView");

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

             builder.HasOne(x => x.Module)
                .WithMany()
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Group)
                .WithMany()
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

             builder.HasIndex(x => new { x.ModuleId, x.GroupId })
                .IsUnique()
                .HasDatabaseName("IX_ModulePermission_Module_Group")
                .HasFilter("[GroupID] IS NOT NULL");

            builder.HasIndex(x => new { x.ModuleId, x.UserId })
                .IsUnique()
                .HasDatabaseName("IX_ModulePermission_Module_User")
                .HasFilter("[UserID] IS NOT NULL");

            builder.HasIndex(x => x.ModuleId)
                .HasDatabaseName("IX_ModulePermission_ModuleId");

            builder.HasIndex(x => x.GroupId)
                .HasDatabaseName("IX_ModulePermission_GroupId");

            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_ModulePermission_UserId");
        }
    }
}