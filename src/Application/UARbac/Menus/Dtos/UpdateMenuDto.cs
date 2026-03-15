namespace Application.UARbac.Menus.Dtos
{
    public sealed record UpdateMenuDto(
        int Id,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        string? Shortcut,
        int? Rank,
        int? ParentId,
        int? FormId,
        int? ObjectId,
        int? ViewFormId,
        bool? IsHide,
        string? Image,
        int? ViewType
    );
}