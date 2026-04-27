namespace Application.System.HRS.Basics.FiscalPeriod.Dtos
{
    public sealed record CreateFiscalYearPeriodDto(
        string? Code,
        int FiscalYearId,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        DateTime? FromDate,
        DateTime? ToDate,
        string? Remarks,
        string? HFromDate,
        string? HToDate,
        byte? PeriodType,
        byte? PeriodRank,
        DateTime? PrepareFromDate,
        DateTime? PrepareToDate,
        int? RegComputerId
    );
}