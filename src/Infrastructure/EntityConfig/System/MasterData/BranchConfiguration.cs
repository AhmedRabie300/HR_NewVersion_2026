using Domain.System.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.MasterData
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("sys_Branches");

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

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasColumnName("CompanyID");

            builder.Property(x => x.CountryId)
                .HasColumnName("CountryID");

            builder.Property(x => x.CityId)
                .HasColumnName("CityID");

            builder.Property(x => x.DefaultAbsent)
                .HasColumnName("DefaultAbsent");

            builder.Property(x => x.PrepareDay)
                .HasColumnName("PrepareDay");

            builder.Property(x => x.AffectPeriod)
                .HasColumnName("AffectPeriod");

            builder.Property(x => x.Remarks)
                .HasMaxLength(2048)
                .HasColumnName("Remarks");

            builder.Property(x => x.RegUserId)
                .HasColumnName("RegUserID");

            builder.Property(x => x.regComputerId)
                .HasMaxLength(50)
                .HasColumnName("regComputerId");

            builder.Property(x => x.RegDate)
                .IsRequired()
                .HasColumnName("RegDate")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

            // Relationships
            builder.HasOne(x => x.Company)
                .WithMany(x => x.Branches)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ParentBranch)
                .WithMany(x => x.ChildBranches)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Branches_Code");

            builder.HasIndex(x => x.EngName)
                .HasDatabaseName("IX_Branches_EngName");

            builder.HasIndex(x => x.ArbName)
                .HasDatabaseName("IX_Branches_ArbName");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_Branches_CompanyId");

            builder.HasIndex(x => x.ParentId)
                .HasDatabaseName("IX_Branches_ParentId");

            builder.HasIndex(x => x.CityId)
                .HasDatabaseName("IX_Branches_CityId");
        }
    }
}