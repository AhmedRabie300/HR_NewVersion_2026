using Domain.Common;

namespace Domain.System.MasterData
{
    public class Profession : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public string? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        public Company? Company { get; private set; }

        private Profession() { }

        public Profession(
            string code,
            int companyId,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? remarks,
            int? regUserId,
            string? regComputerId)
        {
            Code = code;
            CompanyId = companyId;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            Remarks = remarks;
            RegUserId = regUserId;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(string? engName, string? arbName, string? arbName4S, string? remarks)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (remarks != null) Remarks = remarks;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}