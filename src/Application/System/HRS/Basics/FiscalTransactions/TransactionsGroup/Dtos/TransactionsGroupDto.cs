namespace Application.System.HRS.Basics.FiscalTransactions.TransactionsGroup.Dtos
{
    public sealed record TransactionsGroupDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int CompanyId,
        string? CompanyName,
        string? Remarks,
        int? RegComputerId,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}