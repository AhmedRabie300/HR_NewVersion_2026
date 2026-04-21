namespace Application.System.HRS.Basics.FiscalTransactions.Interval.Dtos
{
    public sealed record UpdateIntervalDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? Number,
        string? Remarks
    );
}