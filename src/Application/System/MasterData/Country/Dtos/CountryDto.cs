namespace Application.System.MasterData.Country.Dtos
{
    public sealed record CountryDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? CurrencyId,
        string? CurrencyName,
        int? NationalityId,
        string? NationalityName,
        string? PhoneKey,
        bool? IsMainCountries,
        string? Remarks,
        int? RegionId,
        string? RegionName,
        string? ISOAlpha2,
        string? ISOAlpha3,
        string? Languages,
        string? Continent,
        int? CapitalId,
        string? CapitalName,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}