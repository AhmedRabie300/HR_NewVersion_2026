namespace Application.System.MasterData.Company.Dtos
{
    public sealed record UpdateCompanyDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsHigry,
        bool? IncludeAbsencDays,
        string? EmpFirstName,
        string? EmpSecondName,
        string? EmpThirdName,
        string? EmpFourthName,
        string? EmpNameSeparator,
        string? Remarks,
        int? PrepareDay,
        string? DefaultTheme,
        bool? VacationIsAccum,
        bool? HasSequence,
        int? SequenceLength,
        int? Prefix,
        string? Separator,
        byte? SalaryCalculation,
        bool? DefaultAttend,
        bool? CountEmployeeVacationDaysTotal,
        bool? ZeroBalAfterVac,
        bool? VacSettlement,
        bool? AllowOverVacation,
        bool? VacationFromPrepareDay,
        int? ExecuseRequestHoursallowed,
        bool? EmployeeDocumentsAutoSerial,
        bool? UserDepartmentsPermissions
    );
}