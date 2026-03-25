namespace Application.System.MasterData.ContractType.Dtos
{
    public sealed record UpdateContractTypeDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsSpecial,
        string? Remarks
    );
}