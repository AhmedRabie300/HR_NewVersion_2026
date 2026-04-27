using Domain.Common;
using Domain.System.MasterData;

namespace Domain.System.HRS.Basics.GradesAndClasses
{
    public class Grade : LegacyEntity, ICompanyScoped
    {
        public string? Code { get; private set; }
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int? GradeLevel { get; private set; }
        public decimal? FromSalary { get; private set; }
        public decimal? ToSalary { get; private set; }
        public decimal? RegularHours { get; private set; }
        public int? OverTimeTypeId { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int CompanyId { get; private set; }

        // Navigation properties
        public Company Company { get; private set; }
        private readonly List<GradeTransaction> _transactions = new();
        public IReadOnlyCollection<GradeTransaction> Transactions => _transactions.AsReadOnly();

        private Grade() { }

        public Grade(
            string? code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int? gradeLevel,
            decimal? fromSalary,
            decimal? toSalary,
            decimal? regularHours,
            int? overTimeTypeId,
            int companyId,
            string? remarks,
            int? regComputerId = null)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            GradeLevel = gradeLevel;
            FromSalary = fromSalary;
            ToSalary = toSalary;
            RegularHours = regularHours;
            OverTimeTypeId = overTimeTypeId;
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
            int? gradeLevel,
            decimal? fromSalary,
            decimal? toSalary,
            decimal? regularHours,
            int? overTimeTypeId,
            string? remarks)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (gradeLevel.HasValue) GradeLevel = gradeLevel.Value;
            if (fromSalary.HasValue) FromSalary = fromSalary.Value;
            if (toSalary.HasValue) ToSalary = toSalary.Value;
            if (regularHours.HasValue) RegularHours = regularHours.Value;
            if (overTimeTypeId.HasValue) OverTimeTypeId = overTimeTypeId.Value;
            if (remarks != null) Remarks = remarks;
        }

        public void AddTransaction(GradeTransaction transaction)
        {
            _transactions.Add(transaction);
        }

        public void RemoveTransaction(GradeTransaction transaction)
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