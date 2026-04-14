namespace Application.System.MasterData.City.Dtos
{
    public sealed record CityDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? PhoneKey,
        int? RegionId,
        string? RegionName,
        string? TimeZone,
        int? CountryId,
        string? CountryName,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}