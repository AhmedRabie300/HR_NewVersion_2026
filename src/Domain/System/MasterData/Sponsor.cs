using Domain.Common;

namespace Domain.System.MasterData
{
    public class Sponsor : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int? SponsorNumber { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public int? CompanyId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }

        private Sponsor() { }

        public Sponsor(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int? sponsorNumber,
            int? regUserId,
            int? regComputerId,
            int? companyId)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            SponsorNumber = sponsorNumber;
            RegUserId = regUserId;
            RegComputerId = regComputerId;
            CompanyId = companyId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? engName,
            string? arbName,
            string? arbName4S,
            int? sponsorNumber)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (sponsorNumber.HasValue) SponsorNumber = sponsorNumber;
        }

        public void UpdateCompany(int? companyId)
        {
            if (companyId.HasValue) CompanyId = companyId;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}