namespace Application.System.MasterData.BloodGroup.Dtos
{
    public sealed record CreateBloodGroupDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        int? RegUserId,
        string? RegComputerId
    );
}