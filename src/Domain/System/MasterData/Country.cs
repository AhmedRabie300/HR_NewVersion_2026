using Domain.Common;

namespace Domain.System.MasterData
{
    public class Country : LegacyEntity,ICompanyScoped
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int? CurrencyId { get; private set; }
        public int? NationalityId { get; private set; }
        public string? PhoneKey { get; private set; }
        public bool? IsMainCountries { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? RegionId { get; private set; }
        public string? ISOAlpha2 { get; private set; }
        public string? ISOAlpha3 { get; private set; }
        public string? Languages { get; private set; }
        public string? Continent { get; private set; }
        public int? CapitalId { get; private set; }

        // Navigation properties
        public Currency? Currency { get; private set; }
        public Nationality? Nationality { get; private set; }
        public Region? Region { get; private set; }
        public City? Capital { get; private set; }
        public int CompanyId { get; private set; }

        private Country() { }

        public Country(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int? currencyId,
            int? nationalityId,
            string? phoneKey,
            bool? isMainCountries,
            string? remarks,
            int? regionId,
            string? isoAlpha2,
            string? isoAlpha3,
            string? languages,
            string? continent,
            int? capitalId)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            CurrencyId = currencyId;
            NationalityId = nationalityId;
            PhoneKey = phoneKey;
            IsMainCountries = isMainCountries;
            Remarks = remarks;
            RegionId = regionId;
            ISOAlpha2 = isoAlpha2;
            ISOAlpha3 = isoAlpha3;
            Languages = languages;
            Continent = continent;
            CapitalId = capitalId;
        }

        public void Update(
            string? engName,
            string? arbName,
            string? arbName4S,
            int? currencyId,
            int? nationalityId,
            string? phoneKey,
            bool? isMainCountries,
            string? remarks,
            int? regionId,
            string? isoAlpha2,
            string? isoAlpha3,
            string? languages,
            string? continent,
            int? capitalId)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (currencyId.HasValue) CurrencyId = currencyId;
            if (nationalityId.HasValue) NationalityId = nationalityId;
            if (phoneKey != null) PhoneKey = phoneKey;
            if (isMainCountries.HasValue) IsMainCountries = isMainCountries;
            if (remarks != null) Remarks = remarks;
            if (regionId.HasValue) RegionId = regionId;
            if (isoAlpha2 != null) ISOAlpha2 = isoAlpha2;
            if (isoAlpha3 != null) ISOAlpha3 = isoAlpha3;
            if (languages != null) Languages = languages;
            if (continent != null) Continent = continent;
            if (capitalId.HasValue) CapitalId = capitalId;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}