// Infrastructure/EntityConfig/UARbac/FormConfiguration.cs
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.UARbac
{
    public class FormConfiguration : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder)
        {
            builder.ToTable("sys_Forms");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .HasMaxLength(50);

            builder.Property(x => x.EngName)
                .HasMaxLength(200);

            builder.Property(x => x.ArbName)
                .HasMaxLength(200);

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(200);

            builder.Property(x => x.EngDescription)
                .HasMaxLength(500);

            builder.Property(x => x.ArbDescription)
                .HasMaxLength(500);

            builder.Property(x => x.Remarks)
                .HasMaxLength(500);

            builder.Property(x => x.RegComputerId)
                .HasMaxLength(50);

            builder.Property(x => x.Layout)
                .HasMaxLength(500);

            builder.Property(x => x.LinkTarget)
                .HasMaxLength(100);

            builder.Property(x => x.LinkUrl)
                .HasMaxLength(500);

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            builder.Property(x => x.RegDate)
                .IsRequired();

            // Relationships
            //builder.HasOne(x => x.Module)
            //    .WithMany()
            //    .HasForeignKey(x => x.ModuleId)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SearchForm)
                .WithMany()
                .HasForeignKey(x => x.SearchFormId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.MainForm)
                .WithMany()
                .HasForeignKey(x => x.MainId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Code)
                .HasDatabaseName("IX_Forms_Code");

            builder.HasIndex(x => x.ModuleId)
                .HasDatabaseName("IX_Forms_ModuleId");

            builder.HasIndex(x => x.Rank)
                .HasDatabaseName("IX_Forms_Rank");
        }
    }
}