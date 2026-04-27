using Domain.Common;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics.FiscalPeriod
{
    public class FiscalYearPeriod : LegacyEntity, ICompanyScoped
    {
        public string? Code { get; private set; }
        public int FiscalYearId { get; private set; }
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public DateTime? FromDate { get; private set; }
        public DateTime? ToDate { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public string? HFromDate { get; private set; }
        public string? HToDate { get; private set; }
        public byte? PeriodType { get; private set; }
        public byte? PeriodRank { get; private set; }
        public DateTime? PrepareFromDate { get; private set; }
        public DateTime? PrepareToDate { get; private set; }
        public int CompanyId { get; private set; }

        // Navigation properties
        public FiscalYear? FiscalYear { get; private set; }
        public Company? Company { get; private set; }
        private readonly List<FiscalYearPeriodModule> _modules = new();
        public IReadOnlyCollection<FiscalYearPeriodModule> Modules => _modules.AsReadOnly();

        private FiscalYearPeriod() { }

        public FiscalYearPeriod(
            string? code,
            int fiscalYearId,
            string? engName,
            string? arbName,
            string? arbName4S,
            DateTime? fromDate,
            DateTime? toDate,
            string? remarks,
            string? hFromDate,
            string? hToDate,
            byte? periodType,
            byte? periodRank,
            DateTime? prepareFromDate,
            DateTime? prepareToDate,
            int companyId,
            int? regComputerId = null)
        {
            Code = code;
            FiscalYearId = fiscalYearId;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            FromDate = fromDate;
            ToDate = toDate;
            Remarks = remarks;
            HFromDate = hFromDate;
            HToDate = hToDate;
            PeriodType = periodType;
            PeriodRank = periodRank;
            PrepareFromDate = prepareFromDate;
            PrepareToDate = prepareToDate;
            CompanyId = companyId;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? code,
            string? engName,
            string? arbName,
            string? arbName4S,
            DateTime? fromDate,
            DateTime? toDate,
            string? remarks,
            string? hFromDate,
            string? hToDate,
            byte? periodType,
            byte? periodRank,
            DateTime? prepareFromDate,
            DateTime? prepareToDate)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (fromDate.HasValue) FromDate = fromDate.Value;
            if (toDate.HasValue) ToDate = toDate.Value;
            if (remarks != null) Remarks = remarks;
            if (hFromDate != null) HFromDate = hFromDate;
            if (hToDate != null) HToDate = hToDate;
            if (periodType.HasValue) PeriodType = periodType.Value;
            if (periodRank.HasValue) PeriodRank = periodRank.Value;
            if (prepareFromDate.HasValue) PrepareFromDate = prepareFromDate.Value;
            if (prepareToDate.HasValue) PrepareToDate = prepareToDate.Value;
        }

        public void AddModule(FiscalYearPeriodModule module)
        {
            _modules.Add(module);
        }

        public void RemoveModule(FiscalYearPeriodModule module)
        {
            _modules.Remove(module);
        }

        public void ClearModules()
        {
            _modules.Clear();
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}