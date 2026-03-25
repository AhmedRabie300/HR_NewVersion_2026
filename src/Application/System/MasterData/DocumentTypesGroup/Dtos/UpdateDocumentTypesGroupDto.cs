namespace Application.System.MasterData.DocumentTypesGroup.Dtos
{
    public sealed record UpdateDocumentTypesGroupDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? Remarks
    );
}