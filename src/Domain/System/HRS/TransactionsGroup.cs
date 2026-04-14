using Domain.Common;
using Domain.System.MasterData;

namespace Domain.System.HRS
{
    public class TransactionsGroup : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }

        private TransactionsGroup() { }

        public TransactionsGroup(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int companyId,
            string? remarks,
            int regUserId,
            int? regComputerId)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            CompanyId = companyId;
            Remarks = remarks;
            RegUserId = regUserId;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? code,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? remarks)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (remarks != null) Remarks = remarks;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId.Value;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}