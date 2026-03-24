using Domain.Common;

namespace Domain.System.MasterData
{
    public class MaritalStatus : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        private MaritalStatus() { }

        public MaritalStatus(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? remarks,
            int? regUserId,
            int? regComputerId)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            Remarks = remarks;
            RegUserId = regUserId;
            regComputerId = regComputerId;
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