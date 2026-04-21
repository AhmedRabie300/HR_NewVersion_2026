using Domain.System.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.MasterData
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("hrs_Projects", tb => tb.UseSqlOutputClause(false));


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

            builder.Property(x => x.Phone)
                .HasMaxLength(50)
                .HasColumnName("Phone");

            builder.Property(x => x.Mobile)
                .HasMaxLength(50)
                .HasColumnName("Mobile");

            builder.Property(x => x.Fax)
                .HasMaxLength(50)
                .HasColumnName("Fax");

            builder.Property(x => x.Email)
                .HasMaxLength(100)
                .HasColumnName("Email");

            builder.Property(x => x.Adress)
                .HasMaxLength(1024)
                .HasColumnName("Adress");

            builder.Property(x => x.ContactPerson)
                .HasMaxLength(100)
                .HasColumnName("ContactPerson");

            builder.Property(x => x.ProjectPeriod)
                .HasColumnName("ProjectPeriod");

            builder.Property(x => x.ClaimDuration)
                .HasColumnName("ClaimDuration");

            builder.Property(x => x.StartDate)
                .HasColumnName("StartDate");

            builder.Property(x => x.EndDate)
                .HasColumnName("EndDate");

            builder.Property(x => x.CreditLimit)
                .HasColumnType("money")
                .HasColumnName("CreditLimit");

            builder.Property(x => x.CreditPeriod)
                .HasColumnName("CreditPeriod");

            builder.Property(x => x.IsAdvance)
                .HasColumnName("IsAdvance");

            builder.Property(x => x.IsHijri)
                .HasColumnName("IsHijri");

            builder.Property(x => x.NotifyPeriod)
                .HasColumnName("NotifyPeriod");

            builder.Property(x => x.CompanyConditions)
                .HasMaxLength(8000)
                .HasColumnName("CompanyConditions");

            builder.Property(x => x.ClientConditions)
                .HasMaxLength(8000)
                .HasColumnName("ClientConditions");

            builder.Property(x => x.IsLocked)
                .HasColumnName("IsLocked");

            builder.Property(x => x.IsStoped)
                .HasColumnName("IsStoped");

            builder.Property(x => x.BranchId)
                .HasColumnName("BranchID");

            builder.Property(x => x.CompanyId)
                .IsRequired()
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

            builder.Property(x => x.WorkConditions)
                .HasMaxLength(8000)
                .HasColumnName("WorkConditions");

            builder.Property(x => x.LocationId)
                .HasColumnName("LocationID");

            builder.Property(x => x.AbsentTransaction)
                .HasColumnName("AbsentTransaction");

            builder.Property(x => x.LeaveTransaction)
                .HasColumnName("LeaveTransaction");

            builder.Property(x => x.LateTransaction)
                .HasColumnName("LateTransaction");

            builder.Property(x => x.SickTransaction)
                .HasColumnName("SickTransaction");

            builder.Property(x => x.OTTransaction)
                .HasColumnName("OTTransaction");

            builder.Property(x => x.HOTTransaction)
                .HasColumnName("HOTTransaction");

            builder.Property(x => x.CostCenterCode1)
                .HasMaxLength(50)
                .HasColumnName("CostCenterCode1");

            builder.Property(x => x.DepartmentId)
                .HasColumnName("DepartmentID");

            builder.Property(x => x.CostCenterCode2)
                .HasMaxLength(50)
                .HasColumnName("CostCenterCode2");

            builder.Property(x => x.CostCenterCode3)
                .HasMaxLength(50)
                .HasColumnName("CostCenterCode3");

            builder.Property(x => x.CostCenterCode4)
                .HasMaxLength(50)
                .HasColumnName("CostCenterCode4");

            // Relationships
            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Projects_Code");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_Projects_CompanyId");

            builder.HasIndex(x => x.BranchId)
                .HasDatabaseName("IX_Projects_BranchId");

            builder.HasIndex(x => x.DepartmentId)
                .HasDatabaseName("IX_Projects_DepartmentId");

            builder.HasIndex(x => x.LocationId)
                .HasDatabaseName("IX_Projects_LocationId");
        }
    }
}