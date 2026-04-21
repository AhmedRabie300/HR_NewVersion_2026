using Domain.Common;
using Domain.System.MasterData;

namespace Domain.System.HRS.VacationAndEndOfService
{
    public class EndOfService : LegacyEntity, ICompanyScoped
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? ExtraTransactionId { get; private set; }
        public bool? ExcludedFromSSRequests { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }
        private readonly List<EndOfServiceRule> _rules = new();
        public IReadOnlyCollection<EndOfServiceRule> Rules => _rules.AsReadOnly();

        private EndOfService() { }

        public EndOfService(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int companyId,
            string? remarks,
            int? regComputerId = null,
            int? extraTransactionId = null,
            bool? excludedFromSSRequests = null)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            CompanyId = companyId;
            Remarks = remarks;
            RegComputerId = regComputerId;
            ExtraTransactionId = extraTransactionId;
            ExcludedFromSSRequests = excludedFromSSRequests;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? code,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? remarks,
            int? extraTransactionId,
            bool? excludedFromSSRequests)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (remarks != null) Remarks = remarks;
            if (extraTransactionId.HasValue) ExtraTransactionId = extraTransactionId.Value;
            if (excludedFromSSRequests.HasValue) ExcludedFromSSRequests = excludedFromSSRequests.Value;
        }

        public void AddRule(EndOfServiceRule rule)
        {
            _rules.Add(rule);
        }

        public void RemoveRule(EndOfServiceRule rule)
        {
            _rules.Remove(rule);
        }

        public void ClearRules()
        {
            _rules.Clear();
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}