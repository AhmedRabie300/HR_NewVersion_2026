// Application/UARbac/Forms/Dtos/FormDetailsDto.cs
namespace Application.UARbac.Forms.Dtos
{
    public sealed record FormDetailsDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? EngDescription,
        string? ArbDescription,
        int? Rank,
        int ModuleId,
        string? ModuleName,
        int? SearchFormId,
        string? SearchFormName,
        int? Height,
        int? Width,
        string? Remarks,
        int? RegUserId,
        int? regComputerId,
        DateTime RegDate,
        DateTime? CancelDate,
        string? Layout,
        string? LinkTarget,
        string? LinkUrl,
        string? ImageUrl,
        int? MainId,
        string? MainFormName
    );
}