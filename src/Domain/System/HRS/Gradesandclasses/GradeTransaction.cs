using Domain.Common;
using Domain.System.HRS.Basics.FiscalTransactions;
using Domain.System.HRS.Basics.GradesAndClasses;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics.GradesAndClasses
{
    public class GradeTransaction : LegacyEntity, ICompanyScoped
    {
        public int GradeId { get; private set; }
        public int TransactionTypeId { get; private set; }
        public decimal? MinValue { get; private set; }
        public decimal? MaxValue { get; private set; }
        public int? PaidAtVacation { get; private set; }
        public bool? OnceAtPeriod { get; private set; }
        public int? IntervalId { get; private set; }
        public int? NumberOfTickets { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int CompanyId { get; private set; }

        // Navigation properties
        public Grade? Grade { get; private set; }
        public TransactionsType? TransactionType { get; private set; }
        public Interval? Interval { get; private set; }
        public Company Company { get; private set; }

        private GradeTransaction() { }

        public GradeTransaction(
            int gradeId,
            int transactionTypeId,
            int companyId,
            decimal? minValue,
            decimal? maxValue,
            int? paidAtVacation,
            bool? onceAtPeriod,
            int? intervalId,
            int? numberOfTickets,
            string? remarks,
            int? regComputerId = null)
        {
            GradeId = gradeId;
            TransactionTypeId = transactionTypeId;
            CompanyId = companyId;
            MinValue = minValue;
            MaxValue = maxValue;
            PaidAtVacation = paidAtVacation;
            OnceAtPeriod = onceAtPeriod;
            IntervalId = intervalId;
            NumberOfTickets = numberOfTickets;
            Remarks = remarks;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            decimal? minValue,
            decimal? maxValue,
            int? paidAtVacation,
            bool? onceAtPeriod,
            int? intervalId,
            int? numberOfTickets,
            string? remarks)
        {
            if (minValue.HasValue) MinValue = minValue.Value;
            if (maxValue.HasValue) MaxValue = maxValue.Value;
            if (paidAtVacation.HasValue) PaidAtVacation = paidAtVacation.Value;
            if (onceAtPeriod.HasValue) OnceAtPeriod = onceAtPeriod.Value;
            if (intervalId.HasValue) IntervalId = intervalId.Value;
            if (numberOfTickets.HasValue) NumberOfTickets = numberOfTickets.Value;
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