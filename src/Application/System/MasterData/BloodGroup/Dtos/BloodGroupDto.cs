namespace Application.System.MasterData.BloodGroup.Dtos
{
    public sealed record BloodGroupDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}