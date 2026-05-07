using Application.System.HRS.Contracts.Dtos;

namespace Application.System.HRS.Contracts.Dtos
{
    public sealed record ContractDto(
        int Id,
        int Number,
        int ContractTypeId,
        string? ContractTypeName,
        int EmployeeClassId,
        string? EmployeeClassName,
        int EmployeeId,
        string? EmployeeName,
        DateTime StartDate,
        DateTime? EndDate,
        int? ProfessionId,
        string? ProfessionName,
        int? PositionId,
        string? PositionName,
        int? GradeStepId,
        string? GradeStepName,
        int? CurrencyId,
        string? CurrencyName,
        string? Remarks,
        int? ContractPeriod,
        int CompanyId,
        string? CompanyName,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive,
        bool IsCurrent,
        List<ContractTransactionDto> Transactions
    );
}