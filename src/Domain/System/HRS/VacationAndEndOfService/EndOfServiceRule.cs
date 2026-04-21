using Domain.Common;

namespace Domain.System.HRS.VacationAndEndOfService
{
    public class EndOfServiceRule : LegacyEntity
    {
        public int EndOfServiceId { get; private set; }
        public float? FromWorkingMonths { get; private set; }
        public float? ToWorkingMonths { get; private set; }
        public float? AmountPercent { get; private set; }
        public string? Formula { get; private set; }
        public string? ExtraDedFormula { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation property
        public EndOfService? EndOfService { get; private set; }

        private EndOfServiceRule() { }

        public EndOfServiceRule(
            int endOfServiceId,
            float? fromWorkingMonths,
            float? toWorkingMonths,
            float? amountPercent,
            string? formula,
            string? extraDedFormula,
            string? remarks,
            int? regComputerId = null)
        {
            EndOfServiceId = endOfServiceId;
            FromWorkingMonths = fromWorkingMonths;
            ToWorkingMonths = toWorkingMonths;
            AmountPercent = amountPercent;
            Formula = formula;
            ExtraDedFormula = extraDedFormula;
            Remarks = remarks;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            float? fromWorkingMonths,
            float? toWorkingMonths,
            float? amountPercent,
            string? formula,
            string? extraDedFormula,
            string? remarks)
        {
            if (fromWorkingMonths.HasValue) FromWorkingMonths = fromWorkingMonths.Value;
            if (toWorkingMonths.HasValue) ToWorkingMonths = toWorkingMonths.Value;
            if (amountPercent.HasValue) AmountPercent = amountPercent.Value;
            if (formula != null) Formula = formula;
            if (extraDedFormula != null) ExtraDedFormula = extraDedFormula;
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