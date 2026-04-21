namespace Application.System.HRS.VacationAndEndOfService.VacationsPaidType.Dtos
{
    public sealed record CreateVacationsPaidTypeDto(
        string? Code,
        string? EngName,
        string? ArbName
     );
}