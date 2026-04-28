namespace Application.System.HRS.Basics.FiscalYears.Dtos
{
    public sealed record UpdateFiscalYearDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks
    );
}