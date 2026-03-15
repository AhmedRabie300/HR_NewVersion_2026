// Application/UARbac/Forms/Dtos/CreateFormDto.cs
namespace Application.UARbac.Forms.Dtos
{
    public sealed record CreateFormDto(
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? EngDescription,
        string? ArbDescription,
        int? Rank,
        int ModuleId,
        int? SearchFormId,
        int? Height,
        int? Width,
        string? Remarks,
        int? RegUserId,
        string? RegComputerId,
        string? Layout,
        string? LinkTarget,
        string? LinkUrl,
        string? ImageUrl,
        int? MainId
    );
}