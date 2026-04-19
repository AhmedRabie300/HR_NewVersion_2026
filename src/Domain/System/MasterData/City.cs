using Domain.Common;
using System.Diagnostics.Metrics;

namespace Domain.System.MasterData
{
    public class City : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public string? PhoneKey { get; private set; }
        public int? RegionId { get; private set; }
        public string? TimeZone { get; private set; }
        public int? CountryId { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation properties
        public Country? Country { get; private set; }
        public Region? Region { get; private set; }

        private City() { }

        public City(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? phoneKey,
            int? regionId,
            string? timeZone,
            int? countryId,
            string? remarks)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            PhoneKey = phoneKey;
            RegionId = regionId;
            TimeZone = timeZone;
            CountryId = countryId;
            Remarks = remarks;
        }

        public void Update(
            string? engName,
            string? arbName,
            string? arbName4S,
            string? phoneKey,
            int? regionId,
            string? timeZone,
            int? countryId,
            string? remarks)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (phoneKey != null) PhoneKey = phoneKey;
            if (regionId.HasValue) RegionId = regionId;
            if (timeZone != null) TimeZone = timeZone;
            if (countryId.HasValue) CountryId = countryId;
            if (remarks != null) Remarks = remarks;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}