using Domain.System.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.MasterData
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("sys_Companies");

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

            builder.Property(x => x.IsHigry)
                .HasColumnName("IsHigry");

            builder.Property(x => x.IncludeAbsencDays)
                .HasColumnName("IncludeAbsencDays");

            builder.Property(x => x.EmpFirstName)
                .HasMaxLength(10)
                .HasColumnName("EmpFirstName");

            builder.Property(x => x.EmpSecondName)
                .HasMaxLength(10)
                .HasColumnName("EmpSecondName");

            builder.Property(x => x.EmpThirdName)
                .HasMaxLength(10)
                .HasColumnName("EmpThirdName");

            builder.Property(x => x.EmpFourthName)
                .HasMaxLength(10)
                .HasColumnName("EmpFourthName");

            builder.Property(x => x.EmpNameSeparator)
                .HasMaxLength(1)
                .HasColumnName("EmpNameSeparator");

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

            builder.Property(x => x.PrepareDay)
                .HasColumnName("PrepareDay");

            builder.Property(x => x.DefaultTheme)
                .HasMaxLength(50)
                .HasColumnName("DefaultTheme");

            builder.Property(x => x.VacationIsAccum)
                .HasColumnName("VacationIsAccum");

            builder.Property(x => x.HasSequence)
                .HasColumnName("HasSequence");

            builder.Property(x => x.SequenceLength)
                .HasColumnName("SequenceLength");

            builder.Property(x => x.Prefix)
                .HasColumnName("Prefix");

            builder.Property(x => x.Separator)
                .HasMaxLength(1)
                .HasColumnName("Separator");

            builder.Property(x => x.SalaryCalculation)
                .HasColumnName("SalaryCalculation");

            builder.Property(x => x.DefaultAttend)
                .HasColumnName("DefaultAttend");

            builder.Property(x => x.CountEmployeeVacationDaysTotal)
                .HasColumnName("CountEmployeeVacationDaysTotal");

            builder.Property(x => x.ZeroBalAfterVac)
                .HasColumnName("ZeroBalAfterVac");

            builder.Property(x => x.VacSettlement)
                .HasColumnName("VacSettlement");

            builder.Property(x => x.AllowOverVacation)
                .HasColumnName("AllowOverVacation");

            builder.Property(x => x.VacationFromPrepareDay)
                .HasColumnName("VacationFromPrepareDay");

            builder.Property(x => x.ExecuseRequestHoursallowed)
                .HasColumnName("ExecuseRequestHoursallowed");

            builder.Property(x => x.EmployeeDocumentsAutoSerial)
                .HasColumnName("EmployeeDocumentsAutoSerial");

            builder.Property(x => x.UserDepartmentsPermissions)
                .HasColumnName("UserDepartmentsPermissions");

            // Indexes
            builder.HasIndex(x => x.Code)
                .IsUnique()
                .HasDatabaseName("IX_Companies_Code");

            builder.HasIndex(x => x.EngName)
                .HasDatabaseName("IX_Companies_EngName");

            builder.HasIndex(x => x.ArbName)
                .HasDatabaseName("IX_Companies_ArbName");
        }
    }
}