namespace Application.System.MasterData.DependantType.Dtos
{
    public sealed record DependantTypeDto(
        int Id,
        string Code,
        int CompanyId,
        string? CompanyName,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}