namespace Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos
{
    public sealed record CreateEmployeeClassDelayDto(
        int ClassID,
        int? FromMin,
        int? ToMin,
        int? PunishPCT,
        string? Remarks,
        int? RegComputerId
    );
}