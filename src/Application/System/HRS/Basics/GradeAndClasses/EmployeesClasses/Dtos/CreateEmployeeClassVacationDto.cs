namespace Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos
{
    public sealed record CreateEmployeeClassVacationDto(
        int EmployeeClassId,
        int VacationTypeId,
        int DurationDays,
        int? RequiredWorkingMonths,
        float? FromMonth,
        float? ToMonth,
        string? Remarks,
        int? TicketsRnd,
        int? DependantTicketRnd,
        int? MaxKeepDays,
        int? RegComputerId
    );
}