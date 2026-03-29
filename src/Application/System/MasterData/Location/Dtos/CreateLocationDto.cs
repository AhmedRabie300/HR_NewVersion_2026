namespace Application.System.MasterData.Location.Dtos
{
    public sealed record CreateLocationDto(
        string Code,
        int? CompanyId,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? CityId,
        int? BranchId,
        int? StoreId,
        int? DepartmentId,
        string? Remarks,
        string? CostCenterCode1,
        string? CostCenterCode2,
        string? CostCenterCode3,
        string? CostCenterCode4,
        int? RegUserId,
        int? regComputerId
    );
}