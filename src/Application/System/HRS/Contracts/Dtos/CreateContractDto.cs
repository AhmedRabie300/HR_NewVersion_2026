using Application.System.HRS.Contracts.Dtos;

namespace Application.System.HRS.Contracts.Dtos
{
    public sealed record CreateContractDto(
        int? Number,  // Optional - will be generated if null
        int ContractTypeId,
        int EmployeeClassId,
        int EmployeeId,
        DateTime StartDate,
        DateTime? EndDate,
        int? ProfessionId,
        int? PositionId,
        int? GradeStepId,
        int? CurrencyId,
        string? Remarks,
        int? RegComputerId,
        int? ContractPeriod,
        List<CreateContractTransactionDto>? Transactions
    );
}