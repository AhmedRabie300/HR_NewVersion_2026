namespace Application.System.HRS.Employees.Dtos
{
    public sealed record EmployeeListDto(
        int Id,
        string Code,
        string? FullName,   
        string? DepartmentName,
        string? BranchName,
        string? NationalityName,
        string? PositionName,
        DateTime? JoinDate,
        string? Mobile,
        string? Email,
        bool IsActive
    );
}