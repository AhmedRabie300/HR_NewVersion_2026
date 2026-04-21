namespace Application.System.HRS.VacationAndEndOfService.VacationsPaidType.Dtos
{
    public sealed record UpdateVacationsPaidTypeDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName
    );
}