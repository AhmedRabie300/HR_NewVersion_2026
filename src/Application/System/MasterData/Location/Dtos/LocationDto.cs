// Application/System/MasterData/Location/Dtos/LocationDto.cs
namespace Application.System.MasterData.Location.Dtos
{
    public sealed record LocationDto(
        int Id,
        string Code,
        int? CompanyId,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? CityId,
        string? CityName,
        int? BranchId,
        string? BranchName,
        int? StoreId,
        string? StoreName,
        int? InventoryCostLedgerId,
        string? InventoryCostLedgerName,
        int? InventoryAdjustmentLedgerId,
        string? InventoryAdjustmentLedgerName,
        int? DepartmentId,
        string? DepartmentName,
        string? Remarks,
        string? CostCenterCode1,
        string? CostCenterCode2,
        string? CostCenterCode3,
        string? CostCenterCode4,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    )
    {
        public string? GetDisplayName(int lang) => lang == 2 ? ArbName : EngName;
    }
}