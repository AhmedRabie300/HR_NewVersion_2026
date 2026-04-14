namespace Application.System.MasterData.Country.Dtos
{
    public sealed record CreateCountryDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? CurrencyId,
        int? NationalityId,
        string? PhoneKey,
        bool? IsMainCountries,
        string? Remarks,
        int? RegUserId,
        int? RegComputerId,
        int? RegionId,
        string? ISOAlpha2,
        string? ISOAlpha3,
        string? Languages,
        string? Continent,
        int? CapitalId
    );
}