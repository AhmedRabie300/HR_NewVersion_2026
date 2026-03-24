namespace Application.System.MasterData.DependantType.Dtos
{
    public sealed record CreateDependantTypeDto(
        string Code,
        int CompanyId,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        int? RegUserId,
        string? RegComputerId
    );
}