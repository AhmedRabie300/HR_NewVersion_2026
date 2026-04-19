using Domain.Common;

namespace Domain.System.MasterData
{
    public class Document : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public bool? IsForCompany { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? DocumentTypesGroupId { get; private set; }

        // Navigation properties
        public DocumentTypesGroup? DocumentTypesGroup { get; private set; }

        private Document() { }

        public Document(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            bool? isForCompany,
            string? remarks,
            int? documentTypesGroupId)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            IsForCompany = isForCompany;
            Remarks = remarks;
            DocumentTypesGroupId = documentTypesGroupId;
        }

        public void Update(
            string? engName,
            string? arbName,
            string? arbName4S,
            bool? isForCompany,
            string? remarks,
            int? documentTypesGroupId)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (isForCompany.HasValue) IsForCompany = isForCompany;
            if (remarks != null) Remarks = remarks;
            if (documentTypesGroupId.HasValue) DocumentTypesGroupId = documentTypesGroupId;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}