namespace Application.System.MasterData.Bank.Dtos
{
    public sealed record CreateBankDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks
    );
}