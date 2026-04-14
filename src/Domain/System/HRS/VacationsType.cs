using Domain.Common;
using Domain.System.MasterData;

namespace Domain.System.HRS
{
    public class VacationsType : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public short? IsPaid { get; private set; }
        public string? Sex { get; private set; }
        public bool? IsAnnual { get; private set; }
        public bool? IsSickVacation { get; private set; }
        public bool? IsFromAnnual { get; private set; }
        public int? ForSalaryTransaction { get; private set; }
        public int CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? OBalanceTransactionId { get; private set; }
        public int? OverDueVacationId { get; private set; }
        public float? Stage1Pct { get; private set; }
        public float? Stage2Pct { get; private set; }
        public float? Stage3Pct { get; private set; }
        public int? ForDeductionTransaction { get; private set; }
        public bool? AffectEos { get; private set; }
        public int? VactionTypeCaculation { get; private set; }
        public int? ExceededDaysType { get; private set; }
        public bool? HasPayment { get; private set; }
        public bool? RoundAnnualVacBalance { get; private set; }
        public string? Religion { get; private set; }
        public bool? IsOfficial { get; private set; }
        public bool? OverlapWithAnotherVac { get; private set; }
        public bool? ConsiderAllowedDays { get; private set; }
        public int? TimesNoInYear { get; private set; }
        public int? AllowedDaysNo { get; private set; }
        public bool? ExcludedFromSsRequests { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }

        private VacationsType() { }

        public VacationsType(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            short? isPaid,
            string? sex,
            bool? isAnnual,
            bool? isSickVacation,
            bool? isFromAnnual,
            int? forSalaryTransaction,
            int companyId,
            string? remarks,
            int? regUserId,
            int? regComputerId,
            int? oBalanceTransactionId,
            int? overDueVacationId,
            float? stage1Pct,
            float? stage2Pct,
            float? stage3Pct,
            int? forDeductionTransaction,
            bool? affectEos,
            int? vactionTypeCaculation,
            int? exceededDaysType,
            bool? hasPayment,
            bool? roundAnnualVacBalance,
            string? religion,
            bool? isOfficial,
            bool? overlapWithAnotherVac,
            bool? considerAllowedDays,
            int? timesNoInYear,
            int? allowedDaysNo,
            bool? excludedFromSsRequests)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            IsPaid = isPaid;
            Sex = sex;
            IsAnnual = isAnnual;
            IsSickVacation = isSickVacation;
            IsFromAnnual = isFromAnnual;
            ForSalaryTransaction = forSalaryTransaction;
            CompanyId = companyId;
            Remarks = remarks;
            RegUserId = regUserId;
            RegComputerId = regComputerId;
            OBalanceTransactionId = oBalanceTransactionId;
            OverDueVacationId = overDueVacationId;
            Stage1Pct = stage1Pct;
            Stage2Pct = stage2Pct;
            Stage3Pct = stage3Pct;
            ForDeductionTransaction = forDeductionTransaction;
            AffectEos = affectEos;
            VactionTypeCaculation = vactionTypeCaculation;
            ExceededDaysType = exceededDaysType;
            HasPayment = hasPayment;
            RoundAnnualVacBalance = roundAnnualVacBalance;
            Religion = religion;
            IsOfficial = isOfficial;
            OverlapWithAnotherVac = overlapWithAnotherVac;
            ConsiderAllowedDays = considerAllowedDays;
            TimesNoInYear = timesNoInYear;
            AllowedDaysNo = allowedDaysNo;
            ExcludedFromSsRequests = excludedFromSsRequests;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? code,
            string? engName,
            string? arbName,
            string? arbName4S,
            short? isPaid,
            string? sex,
            bool? isAnnual,
            bool? isSickVacation,
            bool? isFromAnnual,
            int? forSalaryTransaction,
            string? remarks,
            int? oBalanceTransactionId,
            int? overDueVacationId,
            float? stage1Pct,
            float? stage2Pct,
            float? stage3Pct,
            int? forDeductionTransaction,
            bool? affectEos,
            int? vactionTypeCaculation,
            int? exceededDaysType,
            bool? hasPayment,
            bool? roundAnnualVacBalance,
            string? religion,
            bool? isOfficial,
            bool? overlapWithAnotherVac,
            bool? considerAllowedDays,
            int? timesNoInYear,
            int? allowedDaysNo,
            bool? excludedFromSsRequests)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (isPaid.HasValue) IsPaid = isPaid;
            if (sex != null) Sex = sex;
            if (isAnnual.HasValue) IsAnnual = isAnnual;
            if (isSickVacation.HasValue) IsSickVacation = isSickVacation;
            if (isFromAnnual.HasValue) IsFromAnnual = isFromAnnual;
            if (forSalaryTransaction.HasValue) ForSalaryTransaction = forSalaryTransaction;
            if (remarks != null) Remarks = remarks;
            if (oBalanceTransactionId.HasValue) OBalanceTransactionId = oBalanceTransactionId;
            if (overDueVacationId.HasValue) OverDueVacationId = overDueVacationId;
            if (stage1Pct.HasValue) Stage1Pct = stage1Pct;
            if (stage2Pct.HasValue) Stage2Pct = stage2Pct;
            if (stage3Pct.HasValue) Stage3Pct = stage3Pct;
            if (forDeductionTransaction.HasValue) ForDeductionTransaction = forDeductionTransaction;
            if (affectEos.HasValue) AffectEos = affectEos;
            if (vactionTypeCaculation.HasValue) VactionTypeCaculation = vactionTypeCaculation;
            if (exceededDaysType.HasValue) ExceededDaysType = exceededDaysType;
            if (hasPayment.HasValue) HasPayment = hasPayment;
            if (roundAnnualVacBalance.HasValue) RoundAnnualVacBalance = roundAnnualVacBalance;
            if (religion != null) Religion = religion;
            if (isOfficial.HasValue) IsOfficial = isOfficial;
            if (overlapWithAnotherVac.HasValue) OverlapWithAnotherVac = overlapWithAnotherVac;
            if (considerAllowedDays.HasValue) ConsiderAllowedDays = considerAllowedDays;
            if (timesNoInYear.HasValue) TimesNoInYear = timesNoInYear;
            if (allowedDaysNo.HasValue) AllowedDaysNo = allowedDaysNo;
            if (excludedFromSsRequests.HasValue) ExcludedFromSsRequests = excludedFromSsRequests;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}