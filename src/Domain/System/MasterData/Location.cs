using Domain.Common;

namespace Domain.System.MasterData
{
    public class Location : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int? CityId { get; private set; }
        public int? BranchId { get; private set; }
        public int? StoreId { get; private set; }
        public int? InventoryCostLedgerId { get; private set; }
        public int? InventoryAdjustmentLedgerId { get; private set; }
        public int? CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public string? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? DepartmentId { get; private set; }
        public string? CostCenterCode1 { get; private set; }
        public string? CostCenterCode2 { get; private set; }
        public string? CostCenterCode3 { get; private set; }
        public string? CostCenterCode4 { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }
        public Branch? Branch { get; private set; }
        public Department? Department { get; private set; }
        // public City? City { get; private set; } // هتضاف بعدين
        // public Store? Store { get; private set; } // هتضاف بعدين
        // public Ledger? InventoryCostLedger { get; private set; } // هتضاف بعدين
        // public Ledger? InventoryAdjustmentLedger { get; private set; } // هتضاف بعدين

        private Location() { } // For EF Core

        public Location(
            string code,
            int? companyId,
            string? engName,
            string? arbName,
            string? arbName4S,
            int? cityId,
            int? branchId,
            int? storeId,
            int? inventoryCostLedgerId,
            int? inventoryAdjustmentLedgerId,
            int? departmentId,
            string? remarks,
            string? costCenterCode1,
            string? costCenterCode2,
            string? costCenterCode3,
            string? costCenterCode4,
            int? regUserId,
            string? regComputerId)
        {
            Code = code;
            CompanyId = companyId;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            CityId = cityId;
            BranchId = branchId;
            StoreId = storeId;
            InventoryCostLedgerId = inventoryCostLedgerId;
            InventoryAdjustmentLedgerId = inventoryAdjustmentLedgerId;
            DepartmentId = departmentId;
            Remarks = remarks;
            CostCenterCode1 = costCenterCode1;
            CostCenterCode2 = costCenterCode2;
            CostCenterCode3 = costCenterCode3;
            CostCenterCode4 = costCenterCode4;
            RegUserId = regUserId;
            RegComputerId = regComputerId;
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

        public void UpdateRelations(
            int? cityId,
            int? branchId,
            int? storeId,
            int? departmentId)
        {
            if (cityId.HasValue) CityId = cityId;
            if (branchId.HasValue) BranchId = branchId;
            if (storeId.HasValue) StoreId = storeId;
            if (departmentId.HasValue) DepartmentId = departmentId;
        }

        public void UpdateCostCenters(
            string? costCenterCode1,
            string? costCenterCode2,
            string? costCenterCode3,
            string? costCenterCode4)
        {
            if (costCenterCode1 != null) CostCenterCode1 = costCenterCode1;
            if (costCenterCode2 != null) CostCenterCode2 = costCenterCode2;
            if (costCenterCode3 != null) CostCenterCode3 = costCenterCode3;
            if (costCenterCode4 != null) CostCenterCode4 = costCenterCode4;
        }

        public void UpdateLedgers(
            int? inventoryCostLedgerId,
            int? inventoryAdjustmentLedgerId)
        {
            if (inventoryCostLedgerId.HasValue) InventoryCostLedgerId = inventoryCostLedgerId;
            if (inventoryAdjustmentLedgerId.HasValue) InventoryAdjustmentLedgerId = inventoryAdjustmentLedgerId;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}