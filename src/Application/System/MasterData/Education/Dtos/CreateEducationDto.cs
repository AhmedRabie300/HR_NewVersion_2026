namespace Application.System.MasterData.Education.Dtos
{
    public sealed record CreateEducationDto(
        string Code,
        int CompanyId,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? Level,
        double? RequiredYears,
        string? Remarks,
        int? RegUserId,
        string? RegComputerId
    );
}