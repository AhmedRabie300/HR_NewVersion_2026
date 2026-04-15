namespace Application.System.MasterData.Region.Dtos
{
    public sealed record CreateRegionDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int CountryId,
        string? Remarks,
        int? RegComputerId
    );
}