using Domain.System.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.MasterData
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("sys_Documents", tb => tb.UseSqlOutputClause(false));

            builder.ToTable("sys_Documents");

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

            builder.Property(x => x.IsForCompany)
                .HasColumnName("IsForCompany");

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

            builder.Property(x => x.DocumentTypesGroupId)
                .HasColumnName("DocumentTypesGroupId");

            // Relationship with DocumentTypesGroup
            builder.HasOne(x => x.DocumentTypesGroup)
                .WithMany()
                .HasForeignKey(x => x.DocumentTypesGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Documents_Code");

            builder.HasIndex(x => x.DocumentTypesGroupId)
                .HasDatabaseName("IX_Documents_DocumentTypesGroupId");
        }
    }
}