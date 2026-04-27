namespace Application.System.HRS.Basics.FiscalPeriod.Dtos
{
    public sealed record CreateFiscalYearPeriodModuleDto(
        int FiscalYearPeriodId,
        int ModuleId,
        DateTime? OpenDate,
        DateTime? CloseDate,
        string? Remarks,
        int? RegComputerId
    );
}