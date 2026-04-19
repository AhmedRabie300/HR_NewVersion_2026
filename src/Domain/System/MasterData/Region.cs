using Domain.Common;

namespace Domain.System.MasterData
{
    public class Region : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int CountryId { get; private set; }
        public int? CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation properties
        public Country? Country { get; private set; }
        public Company? Company { get; private set; }

        private Region() { }

        public Region(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int countryId,
            string? remarks)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            CountryId = countryId;
            Remarks = remarks;
        }

        public void Update(
            string? engName,
            string? arbName,
            string? arbName4S,
            int? countryId,
            int? companyId,
            string? remarks)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (countryId.HasValue) CountryId = countryId.Value;
            if (companyId.HasValue) CompanyId = companyId;
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