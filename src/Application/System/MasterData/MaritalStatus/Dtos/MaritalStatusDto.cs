namespace Application.System.MasterData.MaritalStatus.Dtos
{
    public sealed record MaritalStatusDto(
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