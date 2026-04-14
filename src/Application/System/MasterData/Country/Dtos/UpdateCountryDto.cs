namespace Application.System.MasterData.Country.Dtos
{
    public sealed record UpdateCountryDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? CurrencyId,
        int? NationalityId,
        string? PhoneKey,
        bool? IsMainCountries,
        string? Remarks,
        int? RegionId,
        string? ISOAlpha2,
        string? ISOAlpha3,
        string? Languages,
        string? Continent,
        int? CapitalId
    );
}