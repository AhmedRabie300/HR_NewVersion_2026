using Application.System.HRS.Basics.FiscalPeriod.Dtos;

namespace Application.System.HRS.Basics.FiscalPeriod.Dtos
{
    public sealed record FiscalYearPeriodDto(
        int Id,
        string? Code,
        int FiscalYearId,
        string? FiscalYearName,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        DateTime? FromDate,
        DateTime? ToDate,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        string? HFromDate,
        string? HToDate,
        byte? PeriodType,
        byte? PeriodRank,
        DateTime? PrepareFromDate,
        DateTime? PrepareToDate,
        int CompanyId,
        string? CompanyName,
        bool IsActive,
        List<FiscalYearPeriodModuleDto> Modules
    );
}