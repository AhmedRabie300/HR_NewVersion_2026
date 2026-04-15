namespace Application.System.HRS.TransactionsGroup.Dtos
{
    public sealed record CreateTransactionsGroupDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        int? RegComputerId
    );
}