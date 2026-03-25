namespace Application.System.MasterData.ContractType.Dtos
{
    public sealed record CreateContractTypeDto(
        string Code,
        int CompanyId,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsSpecial,
        string? Remarks,
        int? RegUserId,
        int? RegComputerId
    );
}