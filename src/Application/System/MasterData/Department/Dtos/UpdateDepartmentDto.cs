namespace Application.System.MasterData.Department.Dtos
{
    public sealed record UpdateDepartmentDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        string? Remarks,
        string? CostCenterCode
    );
}