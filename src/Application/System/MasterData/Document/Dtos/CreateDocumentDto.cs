namespace Application.System.MasterData.Document.Dtos
{
    public sealed record CreateDocumentDto(
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsForCompany,
        string? Remarks,
        int? RegUserId,
        int? RegComputerId,
        int? DocumentTypesGroupId
    );
}