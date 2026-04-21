namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos
{
    public sealed record UpdateEndOfServiceRuleDto(
        int Id,
        int EndOfServiceId,
        float? FromWorkingMonths,
        float? ToWorkingMonths,
        float? AmountPercent,
        string? Formula,
        string? ExtraDedFormula,
        string? Remarks
    );
}