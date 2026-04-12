using Domain.System.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.Search
{
    public class Sys_SearchsConfiguration : IEntityTypeConfiguration<Sys_Searchs>
    {
        public void Configure(EntityTypeBuilder<Sys_Searchs> builder)
        {
            builder.ToTable("sys_Searchs");

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

            builder.Property(x => x.ObjectID)
                .IsRequired()
                .HasColumnName("ObjectID");

            builder.Property(x => x.RegUserID)
                .HasColumnName("RegUserID");

            builder.Property(x => x.RegComputerID)
                .HasMaxLength(50)
                .HasColumnName("RegComputerID");

            builder.Property(x => x.RegDate)
                .IsRequired()
                .HasColumnName("RegDate")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

            // Relationships
            builder.HasOne(x => x.Object)
                .WithMany(x => x.SearchDefinitions)
                .HasForeignKey(x => x.ObjectID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Sys_Searchs_Code");

            builder.HasIndex(x => x.ObjectID)
                .HasDatabaseName("IX_Sys_Searchs_ObjectID");
        }
    }
}