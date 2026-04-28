namespace Application.System.HRS.Basics.FiscalPeriod.Dtos
{
    public sealed record FiscalYearPeriodModuleDto(
        int Id,
        int FiscalYearPeriodId,
        int ModuleId,
        string? ModuleName,
        DateTime? OpenDate,
        DateTime? CloseDate,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive,
        bool IsOpen,
        bool IsClosed
    );
}