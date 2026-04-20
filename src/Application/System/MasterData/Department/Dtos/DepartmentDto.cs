namespace Application.System.MasterData.Department.Dtos
{
    public sealed record DepartmentDto(
        int Id,
        string Code,
        int CompanyId,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        string? ParentDepartmentName,
        string? Remarks,
        string? CostCenterCode,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}