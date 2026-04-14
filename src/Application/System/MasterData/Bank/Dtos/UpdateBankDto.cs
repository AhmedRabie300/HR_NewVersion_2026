namespace Application.System.MasterData.Bank.Dtos
{
    public sealed record UpdateBankDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks
    );
}