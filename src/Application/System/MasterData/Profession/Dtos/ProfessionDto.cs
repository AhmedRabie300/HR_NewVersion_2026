namespace Application.System.MasterData.Profession.Dtos
{
    public sealed record ProfessionDto(
        int Id,
        string Code,
        int CompanyId,
        string? CompanyName,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}