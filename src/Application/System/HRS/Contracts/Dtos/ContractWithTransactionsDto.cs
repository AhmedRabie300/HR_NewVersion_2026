using Application.System.HRS.Contracts.Dtos;

namespace Application.System.HRS.Contracts.Dtos
{
    public sealed record ContractWithTransactionsDto(
        int Id,
        int Number,
        int EmployeeId,
        string? EmployeeName,
        int ContractTypeId,
        string? ContractTypeName,
        int EmployeeClassId,
        string? EmployeeClassName,
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
        int? ContractPeriod,
        string? Remarks,
        bool IsCurrent,
        bool IsActive,
        List<ContractTransactionDto> Transactions
    );
}