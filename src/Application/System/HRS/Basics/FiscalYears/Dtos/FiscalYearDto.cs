namespace Application.System.HRS.Basics.FiscalYears.Dtos
{
    public sealed record FiscalYearDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int CompanyId,
        string? CompanyName,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}