namespace Application.System.MasterData.Sponsor.Dtos
{
    public sealed record CreateSponsorDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? SponsorNumber,
        int? RegUserId,
        int? RegComputerId,
        int? CompanyId
    );
}