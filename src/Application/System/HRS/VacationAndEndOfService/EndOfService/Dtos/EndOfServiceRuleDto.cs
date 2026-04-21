namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos
{
    public sealed record EndOfServiceRuleDto(
        int Id,
        int EndOfServiceId,
        float? FromWorkingMonths,
        float? ToWorkingMonths,
        float? AmountPercent,
        string? Formula,
        string? ExtraDedFormula,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}