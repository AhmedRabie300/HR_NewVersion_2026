namespace Application.System.MasterData.Nationality.Dtos
{
    public sealed record CreateNationalityDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsMainNationality,
        int? TravelRoute,
        int? TravelClass,
        string? Remarks,
        int? RegUserId,
        int? regComputerId,
        double? TicketAmount
    );
}