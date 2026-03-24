namespace Application.System.MasterData.BloodGroup.Dtos
{
    public sealed record UpdateBloodGroupDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks
    );
}