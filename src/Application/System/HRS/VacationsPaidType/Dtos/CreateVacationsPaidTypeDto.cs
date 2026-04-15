namespace Application.System.HRS.VacationsPaidType.Dtos
{
    public sealed record CreateVacationsPaidTypeDto(
        string? Code,
        string? EngName,
        string? ArbName
     );
}