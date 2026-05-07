namespace Application.System.HRS.Contracts.Dtos
{
    public sealed record UpdateContractDto(
        int Id,
        int? ContractTypeId,
        int? EmployeeClassId,
        DateTime? StartDate,
        DateTime? EndDate,
        int? ProfessionId,
        int? PositionId,
        int? GradeStepId,
        int? CurrencyId,
        string? Remarks,
        int? ContractPeriod
    );
}