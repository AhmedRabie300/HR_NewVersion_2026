namespace Application.System.HRS.Basics.FiscalTransactions.TransactionsGroup.Dtos
{
    public sealed record UpdateTransactionsGroupDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks
    );
}