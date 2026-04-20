namespace Application.System.MasterData.Sponsor.Dtos
{
    public sealed record UpdateSponsorDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? SponsorNumber
     
    );
}