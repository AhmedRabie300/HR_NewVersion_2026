// Infrastructure/EntityConfig/UARbac/FormPermissionConfiguration.cs
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.UARbac
{
    public class FormPermissionConfiguration : IEntityTypeConfiguration<FormPermission>
    {
        public void Configure(EntityTypeBuilder<FormPermission> builder)
        {
            builder.ToTable("sys_FormsPermissions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FormId)
                .IsRequired()
                .HasColumnName("FormId");

            builder.Property(x => x.RegComputerId)
                .HasMaxLength(50);

            builder.Property(x => x.RegDate)
                .IsRequired();

            // Relationships
            builder.HasOne(x => x.Form)
                .WithMany(x => x.FormPermissions)  // <- حدد العلاقة مع FormPermissions
                .HasForeignKey(x => x.FormId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Group)
                .WithMany()
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => new { x.FormId, x.GroupId })
                .HasDatabaseName("IX_FormPermissions_Form_Group");

            builder.HasIndex(x => new { x.FormId, x.UserId })
                .HasDatabaseName("IX_FormPermissions_Form_User");

            builder.HasIndex(x => x.GroupId)
                .HasDatabaseName("IX_FormPermissions_GroupId");

            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_FormPermissions_UserId");
        }
    }
}