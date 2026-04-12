using Domain.System.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.Search
{
    public class Sys_SearchsColumnsConfiguration : IEntityTypeConfiguration<Sys_SearchsColumns>
    {
        public void Configure(EntityTypeBuilder<Sys_SearchsColumns> builder)
        {
            builder.ToTable("sys_SearchsColumns");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SearchID)
                .IsRequired()
                .HasColumnName("SearchID");

            builder.Property(x => x.FieldID)
                .IsRequired()
                .HasColumnName("FieldID");

            builder.Property(x => x.EngName)
                .HasMaxLength(100)
                .HasColumnName("EngName");

            builder.Property(x => x.ArbName)
                .HasMaxLength(100)
                .HasColumnName("ArbName");

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(100)
                .HasColumnName("ArbName4S");

            builder.Property(x => x.IsCriteria)
                .HasColumnName("IsCriteria");

            builder.Property(x => x.IsView)
                .HasColumnName("IsView");

            builder.Property(x => x.InputLength)
                .HasColumnName("InputLength");

            builder.Property(x => x.IsArabic)
                .HasColumnName("IsArabic");

            builder.Property(x => x.Alignment)
                .HasColumnName("Alignment");

            builder.Property(x => x.Rank)
                .HasColumnName("Rank");

            builder.Property(x => x.SubSearchID)
                .HasColumnName("SubSearchID");

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

            builder.Property(x => x.RankCriteria)
                .HasColumnName("RankCriteria");

            builder.Property(x => x.RankView)
                .HasColumnName("RankView");

            builder.Property(x => x.IsTarget)
                .HasColumnName("IsTarget");

            // Relationships
            builder.HasOne(x => x.SearchDefinition)
                .WithMany(x => x.Columns)
                .HasForeignKey(x => x.SearchID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Field)
                .WithMany(x => x.SearchColumns)
                .HasForeignKey(x => x.FieldID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SubSearch)
                .WithMany()
                .HasForeignKey(x => x.SubSearchID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.SearchID)
                .HasDatabaseName("IX_Sys_SearchsColumns_SearchID");

            builder.HasIndex(x => x.FieldID)
                .HasDatabaseName("IX_Sys_SearchsColumns_FieldID");

            builder.HasIndex(x => x.SubSearchID)
                .HasDatabaseName("IX_Sys_SearchsColumns_SubSearchID");
        }
    }
}