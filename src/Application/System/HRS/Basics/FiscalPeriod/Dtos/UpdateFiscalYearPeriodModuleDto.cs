namespace Application.System.HRS.Basics.FiscalPeriod.Dtos
{
    public sealed record UpdateFiscalYearPeriodModuleDto(
        int Id,
        int FiscalYearPeriodId,
        int ModuleId,
        DateTime? OpenDate,
        DateTime? CloseDate,
        string? Remarks
    );
}