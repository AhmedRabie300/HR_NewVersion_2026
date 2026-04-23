using Domain.Common;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics.HICompanies
{
    public class HICompanyClass : LegacyEntity, ICompanyScoped
    {
        public int HICompanyId { get; private set; }
        public int CompanyId { get; private set; }   
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public decimal? CompanyAmount { get; private set; }
        public decimal? EmployeeAmount { get; private set; }

        // Navigation properties
        public HICompany? HICompany { get; private set; }
        public Company? Company { get; private set; }

        private HICompanyClass() { }

        public HICompanyClass(
            int hiCompanyId,
            int companyId,   
            string? engName,
            string? arbName,
            string? arbName4S,
            string? remarks,
            decimal? companyAmount,
            decimal? employeeAmount,
            int? regComputerId = null)
        {
            HICompanyId = hiCompanyId;
            CompanyId = companyId;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            Remarks = remarks;
            CompanyAmount = companyAmount;
            EmployeeAmount = employeeAmount;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? engName,
            string? arbName,
            string? arbName4S,
            string? remarks,
            decimal? companyAmount,
            decimal? employeeAmount)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (remarks != null) Remarks = remarks;
            if (companyAmount.HasValue) CompanyAmount = companyAmount.Value;
            if (employeeAmount.HasValue) EmployeeAmount = employeeAmount.Value;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}