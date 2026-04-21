namespace Application.System.HRS.Basics.FiscalTransactions.Interval.Dtos
{
    public sealed record IntervalDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int Number,
        int CompanyId,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}