namespace Application.System.MasterData.Religion.Dtos
{
    public sealed record UpdateReligionDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks
    );
}