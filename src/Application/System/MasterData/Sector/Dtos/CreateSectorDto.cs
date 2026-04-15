namespace Application.System.MasterData.Sector.Dtos
{
    public sealed record CreateSectorDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        string? Remarks,
        int? RegComputerId
    );
}