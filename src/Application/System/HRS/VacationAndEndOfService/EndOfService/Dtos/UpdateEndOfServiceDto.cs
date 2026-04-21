namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos
{
    public sealed record UpdateEndOfServiceDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        int? ExtraTransactionId,
        bool? ExcludedFromSSRequests
    );
}