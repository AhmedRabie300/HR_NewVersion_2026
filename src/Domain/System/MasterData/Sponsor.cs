using Domain.Common;

namespace Domain.System.MasterData
{
    public class Sponsor : LegacyEntity, ICompanyScoped
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int? SponsorNumber { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public int CompanyId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }

        private Sponsor() { }

        public Sponsor(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int? sponsorNumber)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            SponsorNumber = sponsorNumber;
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
            if (sponsorNumber != null) SponsorNumber = sponsorNumber;
        }

        public void UpdateCompany(int companyId)
        {
             CompanyId = companyId;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}