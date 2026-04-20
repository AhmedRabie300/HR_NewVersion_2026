namespace Application.System.MasterData.Education.Dtos
{
    public sealed record CreateEducationDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? Level,
        float? RequiredYears,
        string? Remarks,
        int? regComputerId
    );
}