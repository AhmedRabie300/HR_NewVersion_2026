namespace Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos
{
    public sealed record EmployeeClassDelayDto(
        int Id,
        int? ClassId,
        int? FromMin,
        int? ToMin,
        int? PunishPCT,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}