namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos
{
    public sealed record CreateEndOfServiceRuleDto(
        float? FromWorkingMonths,
        float? ToWorkingMonths,
        float? AmountPercent,
        string? Formula,
        string? ExtraDedFormula,
        string? Remarks,
        int? RegComputerId
    );
}