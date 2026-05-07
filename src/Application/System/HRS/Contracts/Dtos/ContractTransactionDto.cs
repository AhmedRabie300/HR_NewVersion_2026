namespace Application.System.HRS.Contracts.Dtos
{
    public sealed record ContractTransactionDto(
        int Id,
        int ContractId,
        int TransactionTypeId,
        string? TransactionTypeName,
        decimal? Amount,
        bool? Active,
        int? IntervalId,
        string? IntervalName,
        byte? PaidAtVacation,
        bool? OnceAtPeriod,
        string? Remarks,
        DateTime? ActiveDate,
        string? ActiveDateD,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}