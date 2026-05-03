using Domain.System.HRS.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.Employees
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("hrs_Employees", tb => tb.UseSqlOutputClause(false));

            builder.HasKey(x => x.Id);

            // Basic Info
            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnName("Code");

            builder.Property(x => x.OldCode)
                .HasMaxLength(50)
                .HasColumnName("OldCode");

            builder.Property(x => x.EngName)
                .HasMaxLength(100)
                .HasColumnName("EngName");

            builder.Property(x => x.ArbName)
                .HasMaxLength(100)
                .HasColumnName("ArbName");

            builder.Property(x => x.ArbName4S)
                .HasMaxLength(100)
                .HasColumnName("ArbName4S");

            // Family Names
            builder.Property(x => x.FamilyEngName)
                .HasMaxLength(100)
                .HasColumnName("FamilyEngName");

            builder.Property(x => x.FamilyArbName)
                .HasMaxLength(100)
                .HasColumnName("FamilyArbName");

            builder.Property(x => x.FamilyArbName4S)
                .HasMaxLength(100)
                .HasColumnName("FamilyArbName4S");

            // Father Names
            builder.Property(x => x.FatherEngName)
                .HasMaxLength(100)
                .HasColumnName("FatherEngName");

            builder.Property(x => x.FatherArbName)
                .HasMaxLength(100)
                .HasColumnName("FatherArbName");

            builder.Property(x => x.FatherArbName4S)
                .HasMaxLength(100)
                .HasColumnName("FatherArbName4S");

            // Grand Father Names
            builder.Property(x => x.GrandEngName)
                .HasMaxLength(100)
                .HasColumnName("GrandEngName");

            builder.Property(x => x.GrandArbName)
                .HasMaxLength(100)
                .HasColumnName("GrandArbName");

            builder.Property(x => x.GrandArbName4S)
                .HasMaxLength(100)
                .HasColumnName("GrandArbName4S");

            // Personal Info
            builder.Property(x => x.BirthDate)
                .HasColumnName("BirthDate");

            builder.Property(x => x.BirthCityId)
                .HasColumnName("BirthCityID");

            builder.Property(x => x.ReligionId)
                .HasColumnName("ReligionID");

            builder.Property(x => x.MaritalStatusId)
                .HasColumnName("MaritalStatusID");

            builder.Property(x => x.Sex)
                .HasMaxLength(1)
                .HasColumnName("Sex");

            builder.Property(x => x.BloodGroupId)
                .HasColumnName("BloodGroupID");

            // Banking Info
            builder.Property(x => x.BankId)
                .HasColumnName("BankID");

            builder.Property(x => x.NationalityId)
                .HasColumnName("NationalityID");

            builder.Property(x => x.BankAccountNumber)
                .HasMaxLength(100)
                .HasColumnName("BankAccountNumber");

            builder.Property(x => x.BankAccNumber)
                .HasMaxLength(100)
                .HasColumnName("BankAccNumber");

            builder.Property(x => x.BankAccountType)
                .HasMaxLength(50)
                .HasColumnName("BankAccountType");

            // Employment Info
            builder.Property(x => x.DepartmentId)
                .HasColumnName("DepartmentID");

            builder.Property(x => x.BranchId)
                .HasColumnName("BranchID");

            builder.Property(x => x.SectorId)
                .HasColumnName("SectorID");

            builder.Property(x => x.LocationId)
                .HasColumnName("LocationID");

            builder.Property(x => x.SponsorId)
                .HasColumnName("SponsorID");

            builder.Property(x => x.ManagerId)
                .HasColumnName("ManagerID");

            // GOSI Info
            builder.Property(x => x.GOSINumber)
                .HasMaxLength(50)
                .HasColumnName("GOSINumber");

            builder.Property(x => x.GOSIJoinDate)
                .HasColumnName("GOSIJoinDate");

            builder.Property(x => x.GOSIExcludeDate)
                .HasColumnName("GOSIExcludeDate");

            // Dates
            builder.Property(x => x.JoinDate)
                .HasColumnName("JoinDate");

            builder.Property(x => x.ExcludeDate)
                .HasColumnName("ExcludeDate");

            // Contact Info
            builder.Property(x => x.Email)
                .HasMaxLength(255)
                .HasColumnName("E_Mail");

            builder.Property(x => x.WorkEmail)
                .HasMaxLength(255)
                .HasColumnName("WorkE_Mail");

            builder.Property(x => x.Phone)
                .HasMaxLength(100)
                .HasColumnName("Phone");

            builder.Property(x => x.Mobile)
                .HasMaxLength(100)
                .HasColumnName("Mobile");

            // Government IDs
            builder.Property(x => x.SSnNo)
                .HasMaxLength(20)
                .HasColumnName("SSnNo");

            builder.Property(x => x.PassPortNo)
                .HasMaxLength(20)
                .HasColumnName("PassPortNo");

            builder.Property(x => x.EntryNo)
                .HasMaxLength(20)
                .HasColumnName("EntryNo");

            builder.Property(x => x.LaborOfficeNo)
                .HasMaxLength(30)
                .HasColumnName("LaborOfficeNo");

            builder.Property(x => x.MachineCode)
                .HasMaxLength(20)
                .HasColumnName("MachineCode");

            builder.Property(x => x.LedgerCode)
                .HasMaxLength(50)
                .HasColumnName("LedgerCode");

            // Financial
            builder.Property(x => x.WHours)
                .HasColumnName("WHours");

            builder.Property(x => x.MaxLoanDedution)
                .HasColumnName("MaxLoanDedution");

            // Cost Centers
            builder.Property(x => x.Cost1)
                .HasColumnName("Cost1");

            builder.Property(x => x.Cost2)
                .HasColumnName("Cost2");

            builder.Property(x => x.Cost3)
                .HasColumnName("Cost3");

            builder.Property(x => x.Cost4)
                .HasColumnName("Cost4");

            // Booleans
            builder.Property(x => x.IsProjectRelated)
                .HasColumnName("IsProjectRelated");

            builder.Property(x => x.IsSpecialForce)
                .HasColumnName("IsSpecialForce");

            builder.Property(x => x.HasTaqat)
                .HasColumnName("HasTaqat");

            builder.Property(x => x.Hasflexiblesalarydist)
                .HasColumnName("Hasflexiblesalarydist");

            builder.Property(x => x.InsertRequestsForAnotherEmployee)
                .HasColumnName("InsertRequestsForAnotherEmployee");

            builder.Property(x => x.IsSocialInsuranceIncluded)
                .HasColumnName("IsSocialInsuranceIncluded");

            // Payment
            builder.Property(x => x.Paymenttype)
                .HasColumnName("paymenttype");

            // System Fields
            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasColumnName("CompanyID");

            builder.Property(x => x.Remarks)
                .HasMaxLength(2048)
                .HasColumnName("Remarks");

            builder.Property(x => x.RegUserId)
                .IsRequired()
                .HasColumnName("RegUserID");

            builder.Property(x => x.RegComputerId)
                .HasColumnName("RegComputerID");

            builder.Property(x => x.RegDate)
                .IsRequired()
                .HasColumnName("RegDate")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CancelDate)
                .HasColumnName("CancelDate");

            builder.Property(x => x.UpdateUserId)
                .HasColumnName("UpdateUserID");

            builder.Property(x => x.UpdateDate)
                .HasColumnName("UpdateDate");

            builder.Property(x => x.SSNOIssueDate)
                .HasMaxLength(50)
                .HasColumnName("SSNOIssueDate");

            builder.Property(x => x.SSNOExpireDate)
                .HasMaxLength(50)
                .HasColumnName("SSNOExpireDate");

            builder.Property(x => x.PassportIssueDate)
                .HasMaxLength(50)
                .HasColumnName("PassportIssueDate");

            builder.Property(x => x.PassportExpireDate)
                .HasMaxLength(50)
                .HasColumnName("PassportExpireDate");

            builder.Property(x => x.AddressAsPerContract)
                .HasMaxLength(500)
                .HasColumnName("AddressAsPerContract");

            // Relationships
            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.BirthCity)
                .WithMany()
                .HasForeignKey(x => x.BirthCityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Religion)
                .WithMany()
                .HasForeignKey(x => x.ReligionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.MaritalStatus)
                .WithMany()
                .HasForeignKey(x => x.MaritalStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.BloodGroup)
                .WithMany()
                .HasForeignKey(x => x.BloodGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Bank)
                .WithMany()
                .HasForeignKey(x => x.BankId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Nationality)
                .WithMany()
                .HasForeignKey(x => x.NationalityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Sector)
                .WithMany()
                .HasForeignKey(x => x.SectorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Sponsor)
                .WithMany()
                .HasForeignKey(x => x.SponsorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Manager)
                .WithMany()
                .HasForeignKey(x => x.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Employees_Code");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_Employees_CompanyId");

            builder.HasIndex(x => x.DepartmentId)
                .HasDatabaseName("IX_Employees_DepartmentId");

            builder.HasIndex(x => x.BranchId)
                .HasDatabaseName("IX_Employees_BranchId");

            builder.HasIndex(x => x.NationalityId)
                .HasDatabaseName("IX_Employees_NationalityId");

            builder.HasIndex(x => x.SSnNo)
                .HasDatabaseName("IX_Employees_SSnNo");

            builder.HasIndex(x => x.PassPortNo)
                .HasDatabaseName("IX_Employees_PassPortNo");

            builder.HasIndex(x => x.JoinDate)
                .HasDatabaseName("IX_Employees_JoinDate");
        }
    }
}