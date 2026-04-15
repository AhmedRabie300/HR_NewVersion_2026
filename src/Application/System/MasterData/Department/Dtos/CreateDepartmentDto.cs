namespace Application.System.MasterData.Department.Dtos
{
    public sealed record CreateDepartmentDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        string? Remarks,
        string? CostCenterCode,
        int? RegComputerId
    );
}