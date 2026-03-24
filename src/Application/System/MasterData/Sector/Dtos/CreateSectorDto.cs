namespace Application.System.MasterData.Sector.Dtos
{
    public sealed record CreateSectorDto(
        string Code,
        int CompanyId,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        string? Remarks,
        int? RegUserId,
        int? regComputerId
    );
}