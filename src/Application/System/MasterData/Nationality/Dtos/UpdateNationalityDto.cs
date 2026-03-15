namespace Application.System.MasterData.Nationality.Dtos
{
    public sealed record UpdateNationalityDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsMainNationality,
        int? TravelRoute,
        int? TravelClass,
        string? Remarks,
        double? TicketAmount
    );
}