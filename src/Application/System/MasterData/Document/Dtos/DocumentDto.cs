namespace Application.System.MasterData.Document.Dtos
{
    public sealed record DocumentDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        bool? IsForCompany,
        string? Remarks,
        int? DocumentTypesGroupId,
        string? DocumentTypesGroupName,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}