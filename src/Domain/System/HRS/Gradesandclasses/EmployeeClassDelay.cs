using Domain.Common;
using Domain.System.HRS.Basics.GradesAndClasses;

namespace Domain.System.HRS.Basics.GradesAndClasses
{
    public class EmployeeClassDelay : LegacyEntity, ICompanyScoped
    {
        public int? ClassId { get; private set; }
        public int? FromMin { get; private set; }
        public int? ToMin { get; private set; }
        public int? PunishPCT { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int CompanyId { get; private set; }

        // Navigation property
        public EmployeeClass? EmployeeClass { get; private set; }

        private EmployeeClassDelay() { }

        public EmployeeClassDelay(
            int? classId,
            int? fromMin,
            int? toMin,
            int? punishPCT,
            int companyId,
            string? remarks,
            int? regComputerId = null)
        {
            ClassId = classId;
            FromMin = fromMin;
            ToMin = toMin;
            PunishPCT = punishPCT;
            CompanyId = companyId;
            Remarks = remarks;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            int? fromMin,
            int? toMin,
            int? punishPCT,
            string? remarks)
        {
            if (fromMin.HasValue) FromMin = fromMin.Value;
            if (toMin.HasValue) ToMin = toMin.Value;
            if (punishPCT.HasValue) PunishPCT = punishPCT.Value;
            if (remarks != null) Remarks = remarks;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}