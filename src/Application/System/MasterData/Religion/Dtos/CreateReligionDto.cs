namespace Application.System.MasterData.Religion.Dtos
{
    public sealed record CreateReligionDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        int? RegUserId,
        int? regComputerId
    );
}