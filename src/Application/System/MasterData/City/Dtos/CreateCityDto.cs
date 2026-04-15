namespace Application.System.MasterData.City.Dtos
{
    public sealed record CreateCityDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? PhoneKey,
        int? RegionId,
        string? TimeZone,
        int? CountryId,
        string? Remarks,
        int? RegComputerId
    );
}