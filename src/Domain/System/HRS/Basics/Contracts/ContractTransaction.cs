using Domain.Common;
using Domain.System.HRS.Basics.FiscalTransactions;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics.Contracts
{
    public class ContractTransaction : LegacyEntity, ICompanyScoped
    {
        public int ContractId { get; private set; }
        public int TransactionTypeId { get; private set; }
        public decimal? Amount { get; private set; }
        public bool? Active { get; private set; }
        public int? IntervalId { get; private set; }
        public byte? PaidAtVacation { get; private set; }
        public bool? OnceAtPeriod { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public DateTime? ActiveDate { get; private set; }
        public string? ActiveDateD { get; private set; }
        public int CompanyId { get; private set; }

        // Navigation properties
        public Contract? Contract { get; private set; }
        public TransactionsType? TransactionType { get; private set; }
        public Interval? Interval { get; private set; }
        public Company? Company { get; private set; }

        private ContractTransaction() { }

        public ContractTransaction(
            int contractId,
            int transactionTypeId,
             decimal? amount = null,
            bool? active = null,
            int? intervalId = null,
            byte? paidAtVacation = null,
            bool? onceAtPeriod = null,
            string? remarks = null,
             DateTime? activeDate = null,
            string? activeDateD = null)
        {
            ContractId = contractId;
            TransactionTypeId = transactionTypeId;
             Amount = amount;
            Active = active;
            IntervalId = intervalId;
            PaidAtVacation = paidAtVacation;
            OnceAtPeriod = onceAtPeriod;
            Remarks = remarks;
             ActiveDate = activeDate;
            ActiveDateD = activeDateD;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            decimal? amount = null,
            bool? active = null,
            int? intervalId = null,
            byte? paidAtVacation = null,
            bool? onceAtPeriod = null,
            string? remarks = null,
            DateTime? activeDate = null,
            string? activeDateD = null)
        {
            if (amount.HasValue) Amount = amount.Value;
            if (active.HasValue) Active = active.Value;
            if (intervalId.HasValue) IntervalId = intervalId.Value;
            if (paidAtVacation.HasValue) PaidAtVacation = paidAtVacation.Value;
            if (onceAtPeriod.HasValue) OnceAtPeriod = onceAtPeriod.Value;
            if (remarks != null) Remarks = remarks;
            if (activeDate.HasValue) ActiveDate = activeDate.Value;
            if (activeDateD != null) ActiveDateD = activeDateD;
        }

        public void Cancel()
        {
            CancelDate = DateTime.Now;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}