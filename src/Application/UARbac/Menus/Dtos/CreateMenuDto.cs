// Application/UARbac/Menus/Dtos/CreateMenuDto.cs
namespace Application.UARbac.Menus.Dtos
{
    public sealed record CreateMenuDto(
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        string? Shortcut,
        int? Rank,
        int? FormId,
        int? ObjectId,
        int? ViewFormId,
        bool? IsHide,
        string? Image,
        int? ViewType,
        int? RegUserId,
        int? regComputerId
    );
}