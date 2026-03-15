namespace Application.System.MasterData.Nationality.Dtos
{
    public sealed record NationalityDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsMainNationality,
        int? TravelRoute,
        int? TravelClass,
        string? Remarks,
        double? TicketAmount,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}