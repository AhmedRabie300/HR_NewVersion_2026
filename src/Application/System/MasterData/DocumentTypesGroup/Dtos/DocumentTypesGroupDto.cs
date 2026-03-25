namespace Application.System.MasterData.DocumentTypesGroup.Dtos
{
    public sealed record DocumentTypesGroupDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? Remarks,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}