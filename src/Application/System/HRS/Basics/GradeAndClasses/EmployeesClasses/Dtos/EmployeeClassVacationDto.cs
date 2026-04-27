namespace Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos
{
    public sealed record EmployeeClassVacationDto(
        int Id,
        int EmployeeClassId,
        int VacationTypeId,
        string? VacationTypeName,
        int DurationDays,
        int? RequiredWorkingMonths,
        float? FromMonth,
        float? ToMonth,
        string? Remarks,
        int? TicketsRnd,
        int? DependantTicketRnd,
        int? MaxKeepDays,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}