namespace Application.System.MasterData.Sector.Dtos
{
    public sealed record UpdateSectorDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        string? Remarks
    );
}