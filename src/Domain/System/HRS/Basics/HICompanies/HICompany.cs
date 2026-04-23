using Domain.Common;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics.HICompanies
{
    public class HICompany : LegacyEntity, ICompanyScoped
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

         public Company? Company { get; private set; }
        private readonly List<HICompanyClass> _classes = new();
        public IReadOnlyCollection<HICompanyClass> Classes => _classes.AsReadOnly();

        private HICompany() { }

        public HICompany(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int companyId,  
            string? remarks,
            int? regComputerId = null)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            CompanyId = companyId;
            Remarks = remarks;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? code,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? remarks)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (remarks != null) Remarks = remarks;
        }

        public void AddClass(HICompanyClass hiCompanyClass)
        {
            _classes.Add(hiCompanyClass);
        }

        public void RemoveClass(HICompanyClass hiCompanyClass)
        {
            _classes.Remove(hiCompanyClass);
        }

        public void ClearClasses()
        {
            _classes.Clear();
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}