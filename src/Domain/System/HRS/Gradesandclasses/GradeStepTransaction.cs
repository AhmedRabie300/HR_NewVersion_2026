using Domain.Common;
using Domain.System.HRS.Basics.GradesAndClasses;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics.GradesAndClasses
{
    public class GradeStepTransaction : LegacyEntity, ICompanyScoped
    {
        public int GradeStepId { get; private set; }
        public int? GradeTransactionId { get; private set; }
        public decimal? Amount { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public bool? Active { get; private set; }
        public DateTime? ActiveDate { get; private set; }
        public string? ActiveDateD { get; private set; }
        public int CompanyId { get; private set; }

        // Navigation properties
        public GradeStep? GradeStep { get; private set; }
        public GradeTransaction? GradeTransaction { get; private set; }
        public Company? Company { get; private set; }

        private GradeStepTransaction() { }

        public GradeStepTransaction(
            int gradeStepId,
            int? gradeTransactionId,
            int companyId,
            decimal? amount,
            string? remarks,
            bool? active,
            DateTime? activeDate,
            string? activeDateD,
            int? regComputerId = null)
        {
            GradeStepId = gradeStepId;
            GradeTransactionId = gradeTransactionId;
            CompanyId = companyId;
            Amount = amount;
            Remarks = remarks;
            Active = active;
            ActiveDate = activeDate;
            ActiveDateD = activeDateD;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            decimal? amount,
            string? remarks,
            bool? active,
            DateTime? activeDate,
            string? activeDateD)
        {
            if (amount.HasValue) Amount = amount.Value;
            if (remarks != null) Remarks = remarks;
            if (active.HasValue) Active = active.Value;
            if (activeDate.HasValue) ActiveDate = activeDate.Value;
            if (activeDateD != null) ActiveDateD = activeDateD;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}