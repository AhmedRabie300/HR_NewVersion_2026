namespace Application.System.HRS.Contracts.Dtos
{
    public sealed record CreateContractTransactionDto(
        int TransactionTypeId,
        decimal? Amount,
        bool? Active,
        int? IntervalId,
        byte? PaidAtVacation,
        bool? OnceAtPeriod,
        string? Remarks,
        int? RegComputerId,
        DateTime? ActiveDate,
        string? ActiveDateD
    );
}