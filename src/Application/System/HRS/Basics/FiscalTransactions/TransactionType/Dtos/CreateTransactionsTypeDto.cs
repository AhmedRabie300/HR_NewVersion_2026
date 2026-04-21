namespace Application.System.HRS.Basics.FiscalTransactions.TransactionType.Dtos
{
    public sealed record CreateTransactionsTypeDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? ShortEngName,
        string? ShortArbName,
        string? ShortArbName4S,
        int TransactionGroupId,
        short Sign,
        string? DebitAccountCode,
        string? CreditAccountCode,
        bool? IsPaid,
        string? Formula,
        string? BeginContractFormula,
        string? EndContractFormula,
        bool? InputIsNumeric,
        bool? IsEndOfService,
        bool? IsSalaryEOSExeclude,
        bool? IsProjectRelatedItem,
        bool? IsBasicSalary,
        bool? IsDistributable,
        bool? IsAllowPosting,
        string? Remarks,
        int? RegComputerId,
        bool? HasInsuranceTiers
    );
}