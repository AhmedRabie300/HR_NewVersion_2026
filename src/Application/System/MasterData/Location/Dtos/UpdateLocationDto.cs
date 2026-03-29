namespace Application.System.MasterData.Location.Dtos
{
    public sealed record UpdateLocationDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? CityId,
        int? CompanyId,
        int? BranchId,
        int? StoreId,
        int? InventoryCostLedgerId,
        int? InventoryAdjustmentLedgerId,
        int? DepartmentId,
        int? ParentId,
        string? Remarks,
        string? CostCenterCode1,
        string? CostCenterCode2,
        string? CostCenterCode3,
        string? CostCenterCode4
    );
}