using Domain.Common;
using Domain.System.MasterData;
using Domain.UARbac;

namespace Domain.System.HRS.Basics.FiscalPeriod
{
    public class FiscalYearPeriodModule : LegacyEntity, ICompanyScoped
    {
        public int FiscalYearPeriodId { get; private set; }
        public int ModuleId { get; private set; }
        public DateTime? OpenDate { get; private set; }
        public DateTime? CloseDate { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int CompanyId { get; private set; }

        // Navigation properties
        public FiscalYearPeriod? FiscalYearPeriod { get; private set; }
        public Module? Module { get; private set; }
        public Company? Company { get; private set; }

        private FiscalYearPeriodModule() { }

        public FiscalYearPeriodModule(
            int fiscalYearPeriodId,
            int moduleId,
            int companyId,
            DateTime? openDate,
            DateTime? closeDate,
            string? remarks,
            int? regComputerId = null)
        {
            FiscalYearPeriodId = fiscalYearPeriodId;
            ModuleId = moduleId;
            CompanyId = companyId;
            OpenDate = openDate;
            CloseDate = closeDate;
            Remarks = remarks;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(DateTime? openDate, DateTime? closeDate, string? remarks)
        {
            if (openDate.HasValue) OpenDate = openDate.Value;
            if (closeDate.HasValue) CloseDate = closeDate.Value;
            if (remarks != null) Remarks = remarks;
        }

        public void Open(int? userId = null)
        {
            OpenDate = DateTime.Now;
            if (userId.HasValue) RegUserId = userId;
        }

        public void Close(int? userId = null)
        {
            CloseDate = DateTime.Now;
            if (userId.HasValue) RegUserId = userId;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
        public bool IsOpen => OpenDate.HasValue && !CloseDate.HasValue;
        public bool IsClosed => CloseDate.HasValue;
    }
}