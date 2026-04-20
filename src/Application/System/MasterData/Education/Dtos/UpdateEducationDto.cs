namespace Application.System.MasterData.Education.Dtos
{
    public sealed record UpdateEducationDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? Level,
        float? RequiredYears,
        string? Remarks
    );
}