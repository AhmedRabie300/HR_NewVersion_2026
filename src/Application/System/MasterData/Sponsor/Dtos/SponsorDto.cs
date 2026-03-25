namespace Application.System.MasterData.Sponsor.Dtos
{
    public sealed record SponsorDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? SponsorNumber,
        int? CompanyId,
        string? CompanyName,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}