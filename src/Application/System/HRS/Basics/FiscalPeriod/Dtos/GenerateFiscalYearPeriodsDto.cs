namespace Application.System.HRS.Basics.FiscalPeriod.Dtos
{
    public sealed record GenerateFiscalYearPeriodsDto(
        int FiscalYearId,
        bool IsFormal,  
        int? SourceFiscalYearId = null
    );
}