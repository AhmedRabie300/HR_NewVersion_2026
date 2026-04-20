using Domain.Common;

namespace Domain.System.MasterData
{
    public class ContractType : LegacyEntity,ICompanyScoped
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int CompanyId { get; private set; }
        public bool? IsSpecial { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }

        private ContractType() { }

        public ContractType(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            bool? isSpecial,
            string? remarks)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            IsSpecial = isSpecial;
            Remarks = remarks;
        }

        public void Update(
            string? engName,
            string? arbName,
            string? arbName4S,
            bool? isSpecial,
            string? remarks)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (isSpecial.HasValue) IsSpecial = isSpecial;
            if (remarks != null) Remarks = remarks;
        }

        public void UpdateCompany(int? companyId)
        {
            if (companyId.HasValue) CompanyId = companyId.Value;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}