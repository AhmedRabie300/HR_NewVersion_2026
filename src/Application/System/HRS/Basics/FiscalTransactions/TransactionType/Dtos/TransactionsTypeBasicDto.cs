namespace Application.System.HRS.Basics.FiscalTransactions.TransactionType.Dtos
{
    public sealed record TransactionsTypeBasicDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName
    );
}