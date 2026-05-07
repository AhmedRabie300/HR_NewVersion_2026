namespace Application.System.HRS.Contracts.Dtos
{
    public sealed record UpdateContractTransactionDto(
        int Id,
        int ContractId,
        int TransactionTypeId,
        decimal? Amount,
        bool? Active,
        int? IntervalId,
        byte? PaidAtVacation,
        bool? OnceAtPeriod,
        string? Remarks,
        DateTime? ActiveDate,
        string? ActiveDateD
    );
}