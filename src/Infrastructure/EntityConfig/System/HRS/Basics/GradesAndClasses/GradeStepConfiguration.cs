using Domain.System.HRS.Basics.GradesAndClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.GradesAndClasses
{
    public class GradeStepConfiguration : IEntityTypeConfiguration<GradeStep>
    {
        public void Configure(EntityTypeBuilder<GradeStep> builder)
        {
            builder.ToTable("hrs_GradesSteps", tb => tb.UseSqlOutputClause(false));

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

            builder.Property(x => x.GradeId)
                .IsRequired()
                .HasColumnName("GradeID");

            builder.Property(x => x.Step)
                .HasColumnName("Step");

            builder.Property(x => x.CompanyId)
                .HasColumnName("CompanyID");

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

            // Relationships
            builder.HasOne(x => x.Grade)
                .WithMany()
                .HasForeignKey(x => x.GradeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_GradeSteps_Code");

            builder.HasIndex(x => x.GradeId)
                .HasDatabaseName("IX_GradeSteps_GradeId");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_GradeSteps_CompanyId");
        }
    }
}