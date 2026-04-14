namespace Application.System.MasterData.Region.Dtos
{
    public sealed record UpdateRegionDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? CountryId,
        int? CompanyId,
        string? Remarks
    );
}