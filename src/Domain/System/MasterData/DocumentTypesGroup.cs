using Domain.Common;

namespace Domain.System.MasterData
{
    public class DocumentTypesGroup : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        private DocumentTypesGroup() { }

        public DocumentTypesGroup(
            string code,
            string? engName,
            string? arbName,
            string? remarks,
            int? regUserId,
            int? regComputerId)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            Remarks = remarks;
            RegUserId = regUserId;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? engName,
            string? arbName,
            string? remarks)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
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