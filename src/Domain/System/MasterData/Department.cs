 using Domain.Common;

namespace Domain.System.MasterData
{
    public class Department : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int? ParentId { get; private set; }
        public int CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public string? CostCenterCode { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }
        public Department? ParentDepartment { get; private set; }
        public ICollection<Department>? ChildDepartments { get; private set; }

        private Department() { } // For EF Core

        public Department(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int? parentId,
            string? remarks,
            string? costCenterCode)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            ParentId = parentId;
            Remarks = remarks;
            CostCenterCode = costCenterCode;
        }

        // Update methods
        public void UpdateBasicInfo(
            string? engName,
            string? arbName,
            string? arbName4S,
            string? remarks,
            string? costCenterCode)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (remarks != null) Remarks = remarks;
            if (costCenterCode != null) CostCenterCode = costCenterCode;
        }

        public void UpdateParent(int? parentId)
        {
            if (parentId.HasValue) ParentId = parentId;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}