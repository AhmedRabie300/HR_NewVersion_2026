using Domain.Common;
using Domain.System.HRS.Basics.GradesAndClasses;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics.GradesAndClasses
{
    public class GradeStep : LegacyEntity, ICompanyScoped
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int GradeId { get; private set; }
        public int? Step { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int CompanyId { get; private set; }

        // Navigation properties
        public Grade? Grade { get; private set; }
        public Company Company { get; private set; }
        private readonly List<GradeStepTransaction> _transactions = new();
        public IReadOnlyCollection<GradeStepTransaction> Transactions => _transactions.AsReadOnly();

        private GradeStep() { }

        public GradeStep(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int gradeId,
            int? step,
            int companyId,
            string? remarks,
            int? regComputerId = null)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            GradeId = gradeId;
            Step = step;
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
            int? step,
            string? remarks)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (step.HasValue) Step = step.Value;
            if (remarks != null) Remarks = remarks;
        }

        public void AddTransaction(GradeStepTransaction transaction)
        {
            _transactions.Add(transaction);
        }

        public void RemoveTransaction(GradeStepTransaction transaction)
        {
            _transactions.Remove(transaction);
        }

        public void ClearTransactions()
        {
            _transactions.Clear();
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}