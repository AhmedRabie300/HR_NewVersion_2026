using Domain.UARbac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.UARbac
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("sys_Menus");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(50);

            builder.Property(x => x.EngName)
                .HasMaxLength(200);

            builder.Property(x => x.ArbName)
                .HasMaxLength(200);

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(200);

            builder.Property(x => x.Shortcut)
                .HasMaxLength(50);

            builder.Property(x => x.Image)
                .HasMaxLength(500);

            builder.Property(x => x.RegComputerId)
                .HasMaxLength(50);

            builder.Property(x => x.RegDate)
                .IsRequired();

            // Self-referencing relationship (Parent-Child)
            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

 
            // Indexes
            builder.HasIndex(x => x.Code)
                .HasDatabaseName("IX_Menus_Code");

            builder.HasIndex(x => x.ParentId)
                .HasDatabaseName("IX_Menus_ParentId");

            builder.HasIndex(x => x.FormId)
                .HasDatabaseName("IX_Menus_FormId");

            builder.HasIndex(x => x.Rank)
                .HasDatabaseName("IX_Menus_Rank");
        }
    }
}