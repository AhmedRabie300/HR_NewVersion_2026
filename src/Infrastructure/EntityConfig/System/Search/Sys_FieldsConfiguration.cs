using Domain.System.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.Search
{
    public class Sys_FieldsConfiguration : IEntityTypeConfiguration<Sys_Fields>
    {
        public void Configure(EntityTypeBuilder<Sys_Fields> builder)
        {
            builder.ToTable("sys_Fields");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ObjectID)
                .IsRequired()
                .HasColumnName("ObjectID");

            builder.Property(x => x.FieldName)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("FieldName");

            builder.Property(x => x.FieldType)
                .HasColumnName("FieldType");

            builder.Property(x => x.FieldLength)
                .HasColumnName("FieldLength");

            builder.Property(x => x.SysColumns_OrderID)
                .HasColumnName("SysColumns_OrderID");

            builder.Property(x => x.ViewObjectID)
                .HasColumnName("ViewObjectID");

            builder.Property(x => x.ViewEngFieldID)
                .HasColumnName("ViewEngFieldID");

            builder.Property(x => x.ViewArbFieldID)
                .HasColumnName("ViewArbFieldID");

            builder.Property(x => x.RegDate)
                .IsRequired()
                .HasColumnName("RegDate")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

            // Relationships
            builder.HasOne(x => x.Object)
                .WithMany(x => x.Fields)
                .HasForeignKey(x => x.ObjectID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ObjectID)
                .HasDatabaseName("IX_Sys_Fields_ObjectID");
        }
    }
}