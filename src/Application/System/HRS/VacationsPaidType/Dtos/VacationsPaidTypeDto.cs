namespace Application.System.HRS.VacationsPaidType.Dtos
{
    public sealed record VacationsPaidTypeDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}