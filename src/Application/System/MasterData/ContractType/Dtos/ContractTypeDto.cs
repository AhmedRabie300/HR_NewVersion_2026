namespace Application.System.MasterData.ContractType.Dtos
{
    public sealed record ContractTypeDto(
        int Id,
        string Code,
       
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