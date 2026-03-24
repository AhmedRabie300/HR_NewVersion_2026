using Domain.Common;

namespace Domain.System.MasterData
{
    public class Company : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public bool? IsHigry { get; private set; }
        public bool? IncludeAbsencDays { get; private set; }
        public string? EmpFirstName { get; private set; }
        public string? EmpSecondName { get; private set; }
        public string? EmpThirdName { get; private set; }
        public string? EmpFourthName { get; private set; }
        public string? EmpNameSeparator { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? PrepareDay { get; private set; }
        public string? DefaultTheme { get; private set; }
        public bool? VacationIsAccum { get; private set; }
        public bool? HasSequence { get; private set; }
        public int? SequenceLength { get; private set; }
        public int? Prefix { get; private set; }
        public string? Separator { get; private set; }
        public byte? SalaryCalculation { get; private set; }
        public bool? DefaultAttend { get; private set; }
        public bool? CountEmployeeVacationDaysTotal { get; private set; }
        public bool? ZeroBalAfterVac { get; private set; }
        public bool? VacSettlement { get; private set; }
        public bool? AllowOverVacation { get; private set; }
        public bool? VacationFromPrepareDay { get; private set; }
        public int? ExecuseRequestHoursallowed { get; private set; }
        public bool? EmployeeDocumentsAutoSerial { get; private set; }
        public bool? UserDepartmentsPermissions { get; private set; }

        // Navigation properties
        public ICollection<Branch>? Branches { get; private set; }
        public ICollection<Department>? Departments { get; private set; }
        public ICollection<Sector>? Sectors { get; private set; }

        private Company() { } // For EF Core

        public Company(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            bool? isHigry,
            bool? includeAbsencDays,
            string? empFirstName,
            string? empSecondName,
            string? empThirdName,
            string? empFourthName,
            string? empNameSeparator,
            string? remarks,
            int? regUserId,
            int? regComputerId,
            int? prepareDay,
            string? defaultTheme,
            bool? vacationIsAccum,
            bool? hasSequence,
            int? sequenceLength,
            int? prefix,
            string? separator,
            byte? salaryCalculation,
            bool? defaultAttend,
            bool? countEmployeeVacationDaysTotal,
            bool? zeroBalAfterVac,
            bool? vacSettlement,
            bool? allowOverVacation,
            bool? vacationFromPrepareDay,
            int? execuseRequestHoursallowed,
            bool? employeeDocumentsAutoSerial,
            bool? userDepartmentsPermissions)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            IsHigry = isHigry;
            IncludeAbsencDays = includeAbsencDays;
            EmpFirstName = empFirstName;
            EmpSecondName = empSecondName;
            EmpThirdName = empThirdName;
            EmpFourthName = empFourthName;
            EmpNameSeparator = empNameSeparator;
            Remarks = remarks;
            RegUserId = regUserId;
            regComputerId = regComputerId;
            PrepareDay = prepareDay;
            DefaultTheme = defaultTheme;
            VacationIsAccum = vacationIsAccum;
            HasSequence = hasSequence;
            SequenceLength = sequenceLength;
            Prefix = prefix;
            Separator = separator;
            SalaryCalculation = salaryCalculation;
            DefaultAttend = defaultAttend;
            CountEmployeeVacationDaysTotal = countEmployeeVacationDaysTotal;
            ZeroBalAfterVac = zeroBalAfterVac;
            VacSettlement = vacSettlement;
            AllowOverVacation = allowOverVacation;
            VacationFromPrepareDay = vacationFromPrepareDay;
            ExecuseRequestHoursallowed = execuseRequestHoursallowed;
            EmployeeDocumentsAutoSerial = employeeDocumentsAutoSerial;
            UserDepartmentsPermissions = userDepartmentsPermissions;
            RegDate = DateTime.UtcNow;
        }

        // Update methods
        public void UpdateBasicInfo(
            string? engName,
            string? arbName,
            string? arbName4S,
            string? remarks,
            string? defaultTheme)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (remarks != null) Remarks = remarks;
            if (defaultTheme != null) DefaultTheme = defaultTheme;
        }

        public void UpdateEmployeeNaming(
            string? empFirstName,
            string? empSecondName,
            string? empThirdName,
            string? empFourthName,
            string? empNameSeparator)
        {
            if (empFirstName != null) EmpFirstName = empFirstName;
            if (empSecondName != null) EmpSecondName = empSecondName;
            if (empThirdName != null) EmpThirdName = empThirdName;
            if (empFourthName != null) EmpFourthName = empFourthName;
            if (empNameSeparator != null) EmpNameSeparator = empNameSeparator;
        }

        public void UpdateVacationSettings(
            bool? vacationIsAccum,
            bool? zeroBalAfterVac,
            bool? vacSettlement,
            bool? allowOverVacation,
            bool? vacationFromPrepareDay,
            int? execuseRequestHoursallowed)
        {
            if (vacationIsAccum.HasValue) VacationIsAccum = vacationIsAccum;
            if (zeroBalAfterVac.HasValue) ZeroBalAfterVac = zeroBalAfterVac;
            if (vacSettlement.HasValue) VacSettlement = vacSettlement;
            if (allowOverVacation.HasValue) AllowOverVacation = allowOverVacation;
            if (vacationFromPrepareDay.HasValue) VacationFromPrepareDay = vacationFromPrepareDay;
            if (execuseRequestHoursallowed.HasValue) ExecuseRequestHoursallowed = execuseRequestHoursallowed;
        }

        public void UpdateSequenceSettings(
            bool? hasSequence,
            int? sequenceLength,
            int? prefix,
            string? separator)
        {
            if (hasSequence.HasValue) HasSequence = hasSequence;
            if (sequenceLength.HasValue) SequenceLength = sequenceLength;
            if (prefix.HasValue) Prefix = prefix;
            if (separator != null) Separator = separator;
        }

        public void UpdateFlags(
            bool? isHigry,
            bool? includeAbsencDays,
            bool? defaultAttend,
            bool? countEmployeeVacationDaysTotal,
            bool? employeeDocumentsAutoSerial,
            bool? userDepartmentsPermissions)
        {
            if (isHigry.HasValue) IsHigry = isHigry;
            if (includeAbsencDays.HasValue) IncludeAbsencDays = includeAbsencDays;
            if (defaultAttend.HasValue) DefaultAttend = defaultAttend;
            if (countEmployeeVacationDaysTotal.HasValue) CountEmployeeVacationDaysTotal = countEmployeeVacationDaysTotal;
            if (employeeDocumentsAutoSerial.HasValue) EmployeeDocumentsAutoSerial = employeeDocumentsAutoSerial;
            if (userDepartmentsPermissions.HasValue) UserDepartmentsPermissions = userDepartmentsPermissions;
        }

        public void UpdateSalaryCalculation(byte? salaryCalculation)
        {
            if (salaryCalculation.HasValue) SalaryCalculation = salaryCalculation;
        }

        public void UpdatePrepareDay(int? prepareDay)
        {
            if (prepareDay.HasValue) PrepareDay = prepareDay;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}