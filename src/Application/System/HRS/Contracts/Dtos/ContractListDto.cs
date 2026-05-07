namespace Application.System.HRS.Contracts.Dtos
{
    public sealed record ContractListDto(
        int Id,
        int Number,
        int EmployeeID,
        string? EmployeeName,
        string? ContractTypeName,
        DateTime StartDate,
        DateTime? EndDate,
        bool IsCurrent,
        bool IsActive
    );
}