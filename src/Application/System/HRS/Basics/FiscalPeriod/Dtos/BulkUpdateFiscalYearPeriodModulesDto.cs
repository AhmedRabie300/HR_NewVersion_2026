namespace Application.System.HRS.Basics.FiscalPeriod.Dtos
{
    public sealed record BulkUpdateFiscalYearPeriodModulesDto(
        int FiscalYearId,
        Dictionary<int, List<int>> OpenModules   
    );
}