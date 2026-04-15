namespace Application.System.MasterData.DocumentTypesGroup.Dtos
{
    public sealed record CreateDocumentTypesGroupDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? Remarks,
        int? RegComputerId
    );
}