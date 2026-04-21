namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos
{
    public sealed record CreateEndOfServiceDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        int? RegComputerId,
        int? ExtraTransactionId,
        bool? ExcludedFromSSRequests,
        List<CreateEndOfServiceRuleDto> Rules
    );
}