namespace Application.System.MasterData.Education.Dtos
{
    public sealed record EducationDto(
        int Id,
        string Code,
        int CompanyId,
        string? CompanyName,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? Level,
        double? RequiredYears,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}