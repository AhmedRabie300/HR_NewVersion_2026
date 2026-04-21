namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos
{
    public sealed record EndOfServiceDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int CompanyId,
        string? CompanyName,
        string? Remarks,
        int? ExtraTransactionId,
        bool? ExcludedFromSSRequests,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive,
        List<EndOfServiceRuleDto> Rules
    );
}