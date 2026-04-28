namespace Application.System.HRS.Basics.FiscalYears.Dtos
{
    public sealed record CreateFiscalYearDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        int? RegComputerId
    );
}