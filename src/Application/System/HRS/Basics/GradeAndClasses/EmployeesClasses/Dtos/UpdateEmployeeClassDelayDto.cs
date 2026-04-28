namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Dtos
{
    public sealed record UpdateEmployeeClassDelayDto(
        int Id,
        int ClassId,
        int? FromMin,
        int? ToMin,
        int? PunishPCT,
        string? Remarks
    );
}