// Infrastructure/EntityConfig/System/MasterData/PositionConfiguration.cs
using Domain.System.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.MasterData
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("hrs_Positions");

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

            builder.Property(x => x.ParentId)
                .HasColumnName("ParentID");

            builder.Property(x => x.PositionLevelId)
                .HasColumnName("PositionLevelID");

            builder.Property(x => x.EvalEvaluationId)
                .HasColumnName("EvalEvaluationID");

            builder.Property(x => x.EvalRecruitmentId)
                .HasColumnName("EvalRecruitmentID");

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

            builder.Property(x => x.EmployeesNo)
                .HasColumnName("EmployeesNo");

            builder.Property(x => x.ApplyValidation)
                .HasColumnName("ApplyValidation");

            builder.Property(x => x.PositionBudget)
                .HasMaxLength(5)
                .HasColumnName("PositionBudget");

            builder.Property(x => x.AppraisalTypeGroupId)
                .HasColumnName("AppraisalTypeGroupID");

            // Relationships
            builder.HasOne(x => x.ParentPosition)
                .WithMany(x => x.ChildPositions)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Positions_Code");

            builder.HasIndex(x => x.EngName)
                .HasDatabaseName("IX_Positions_EngName");

            builder.HasIndex(x => x.ArbName)
                .HasDatabaseName("IX_Positions_ArbName");

            builder.HasIndex(x => x.ParentId)
                .HasDatabaseName("IX_Positions_ParentId");

            builder.HasIndex(x => x.PositionLevelId)
                .HasDatabaseName("IX_Positions_PositionLevelId");
        }
    }
}