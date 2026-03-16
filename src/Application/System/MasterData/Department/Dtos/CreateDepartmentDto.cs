namespace Application.System.MasterData.Department.Dtos
{
    public sealed record CreateDepartmentDto(
        string Code,
        int CompanyId,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        string? Remarks,
        int? RegUserId,
        string? RegComputerId,
        string? CostCenterCode
    );
}