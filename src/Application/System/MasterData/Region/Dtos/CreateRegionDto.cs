namespace Application.System.MasterData.Region.Dtos
{
    public sealed record CreateRegionDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int CountryId,
        int? CompanyId,
        string? Remarks,
        int? RegUserId,
        int? RegComputerId
    );
}