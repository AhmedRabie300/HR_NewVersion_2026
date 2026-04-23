namespace Application.System.HRS.Basics.ContractsTypes.Dtos
{
    public sealed record CreateContractsTypeDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsSpecial,
        string? Remarks,
        int? RegComputerId
    );
}