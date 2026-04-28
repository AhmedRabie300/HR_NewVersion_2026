using Domain.System.HRS.Basics.GradesAndClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfig.System.HRS.Basics.GradesAndClasses
{
    public class EmployeeClassConfiguration : IEntityTypeConfiguration<EmployeeClass>
    {
        public void Configure(EntityTypeBuilder<EmployeeClass> builder)
        {
            builder.ToTable("hrs_EmployeesClasses", tb => tb.UseSqlOutputClause(false));

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

            builder.Property(x => x.NoOfDaysPerPeriod)
                .HasColumnName("NoOfDaysPerPeriod");

            builder.Property(x => x.WorkHoursPerDay)
    .HasColumnType("real")
    .HasColumnName("WorkHoursPerDay");

            builder.Property(x => x.NoOfHoursPerWeek)
                .HasColumnName("NoOfHoursPerWeek");

            builder.Property(x => x.NoOfHoursPerPeriod)
                .HasColumnName("NoOfHoursPerPeriod");

            builder.Property(x => x.OvertimeFactor)
    .HasColumnType("real")
    .HasColumnName("OvertimeFactor");

            builder.Property(x => x.HolidayFactor)
     .HasColumnType("real")
     .HasColumnName("HolidayFactor");

            builder.Property(x => x.FirstDayOfWeek)
                .HasColumnName("FirstDayOfWeek");

            builder.Property(x => x.DefultStartTime)
                .HasColumnName("DefultStartTime");

            builder.Property(x => x.DefultEndTime)
                .HasColumnName("DefultEndTime");

            builder.Property(x => x.WorkingUnitsIsHours)
                .HasColumnName("WorkingUnitsIsHours");

            builder.Property(x => x.DefaultProjectId)
                .HasColumnName("DefaultProjectID");

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

            builder.Property(x => x.NonPermiLatTransaction)
                .HasColumnName("NonPermiLatTransaction");

            builder.Property(x => x.PerDailyDelaying)
                .HasColumnName("PerDailyDelaying");

            builder.Property(x => x.PerMonthlyDelaying)
                .HasColumnName("PerMonthlyDelaying");

            builder.Property(x => x.NonProfitOverTimeH)
                .HasColumnName("NonProfitOverTimeH");

            builder.Property(x => x.EOBFormula)
                .HasMaxLength(2048)
                .HasColumnName("EOBFormula");

            builder.Property(x => x.OvertimeFormula)
                .HasMaxLength(2048)
                .HasColumnName("OvertimeFormula");

            builder.Property(x => x.HolidayFormula)
                .HasMaxLength(2048)
                .HasColumnName("HolidayFormula");

            builder.Property(x => x.OvertimeTransaction)
                .HasColumnName("OvertimeTransaction");

            builder.Property(x => x.HOvertimeTransaction)
                .HasColumnName("HOvertimeTransaction");

            builder.Property(x => x.PolicyCheckMachine)
                .HasColumnName("PolicyCheckMachine");

            builder.Property(x => x.HasAttendance)
                .HasColumnName("HasAttendance");

            builder.Property(x => x.PunishementCalc)
                .HasColumnName("PunishementCalc");

            builder.Property(x => x.OnNoExit)
                .HasColumnName("OnNoExit");

            builder.Property(x => x.DeductionMethod)
                .HasColumnName("DeductionMethod");

            builder.Property(x => x.MaxLoanAmtPCT)
                .HasColumnName("MaxLoanAmtPCT");

            builder.Property(x => x.MinServiceMonth)
                .HasColumnName("MinServiceMonth");

            builder.Property(x => x.MaxInstallementPCT)
                .HasColumnName("MaxInstallementPCT");

            builder.Property(x => x.EOSCostingTrns)
                .HasColumnName("EOSCostingTrns");

            builder.Property(x => x.TicketsCostingTrns)
                .HasColumnName("TicketsCostingTrns");

            builder.Property(x => x.VacCostingTrns)
                .HasColumnName("VacCostingTrns");

            builder.Property(x => x.HICostingTrns)
                .HasColumnName("HICostingTrns");

            builder.Property(x => x.TravalTrans)
                .HasColumnName("TravalTrans");

            builder.Property(x => x.AbsentFormula)
                .HasMaxLength(2048)
                .HasColumnName("AbsentFormula");

            builder.Property(x => x.LateFormula)
                .HasMaxLength(2048)
                .HasColumnName("LateFormula");

            builder.Property(x => x.VacCostFormula)
                .HasMaxLength(2048)
                .HasColumnName("VacCostFormula");

            builder.Property(x => x.HasFingerPrint)
                .HasColumnName("HasFingerPrint");

            builder.Property(x => x.HasOvertimeList)
                .HasColumnName("HasOvertimeList");

            builder.Property(x => x.AttendanceFromTimeSheet)
                .HasColumnName("AttendanceFromTimeSheet");

            builder.Property(x => x.HasFlexibleTime)
                .HasColumnName("HasFlexibleTime");

            builder.Property(x => x.HasFlexableFingerPrint)
                .HasColumnName("HasFlexableFingerPrint");

            builder.Property(x => x.AdvanceBalance)
                .HasColumnName("AdvanceBalance");

            builder.Property(x => x.VacationTrans)
                .HasColumnName("VacationTrans");

            builder.Property(x => x.VactionTransType)
                .HasColumnName("VactionTransType");

            builder.Property(x => x.TransValue)
                .HasColumnName("TransValue");

            builder.Property(x => x.AddBalanceInAddEmp)
                .HasColumnName("AddBalanceInAddEmp");

            builder.Property(x => x.AccumulatedBalance)
                .HasColumnName("AccumulatedBalance");

            // Relationships
            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DefaultProject)
                .WithMany()
                .HasForeignKey(x => x.DefaultProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => new { x.Code, x.CompanyId })
                .IsUnique()
                .HasDatabaseName("IX_hrs_EmployeeClasses_Unique");

            builder.HasIndex(x => x.CompanyId)
                .HasDatabaseName("IX_EmployeeClasses_CompanyId");

            builder.HasIndex(x => x.DefaultProjectId)
                .HasDatabaseName("IX_EmployeeClasses_DefaultProjectId");
        }
    }
}