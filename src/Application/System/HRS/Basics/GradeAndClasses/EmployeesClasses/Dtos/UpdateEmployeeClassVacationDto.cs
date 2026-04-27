namespace Application.System.HRS.Basics.EmployeesClasses.Dtos
{
    public sealed record UpdateEmployeeClassVacationDto(
        int Id,
        int EmployeeClassId,
        int VacationTypeId,
        int? DurationDays,
        int? RequiredWorkingMonths,
        float? FromMonth,
        float? ToMonth,
        string? Remarks,
        int? TicketsRnd,
        int? DependantTicketRnd,
        int? MaxKeepDays
    );
}