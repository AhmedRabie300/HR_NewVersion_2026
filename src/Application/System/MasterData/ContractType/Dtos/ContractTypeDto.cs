namespace Application.System.MasterData.ContractType.Dtos
{
    public sealed record ContractTypeDto(
        int Id,
        string Code,
        int CompanyId,
        string? CompanyName,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsSpecial,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}