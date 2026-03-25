namespace Application.System.MasterData.Document.Dtos
{
    public sealed record UpdateDocumentDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsForCompany,
        string? Remarks,
        int? DocumentTypesGroupId
    );
}