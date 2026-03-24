// Domain/System/MasterData/Sector.cs
using Domain.Common;

namespace Domain.System.MasterData
{
    public class Sector : LegacyEntity
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

        // Navigation properties
        public Company? Company { get; private set; }
        public Sector? ParentSector { get; private set; }
        public ICollection<Sector>? ChildSectors { get; private set; }

        private Sector() { } // For EF Core

        public Sector(
            string code,
            int companyId,
            string? engName,
            string? arbName,
            string? arbName4S,
            int? parentId,
            string? remarks,
            int? regUserId,
            int? regComputerId)
        {
            Code = code;
            CompanyId = companyId;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            ParentId = parentId;
            Remarks = remarks;
            RegUserId = regUserId;
            regComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        // Update methods
        public void UpdateBasicInfo(
            string? engName,
            string? arbName,
            string? arbName4S,
            string? remarks)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (remarks != null) Remarks = remarks;
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