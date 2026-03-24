namespace Application.UARbac.Menus.Dtos
{
    public sealed record MenuDetailsDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        int? ParentId,
        string? ParentName,
        string? Shortcut,
        int? Rank,
        int? FormId,
        int? ObjectId,
        int? ViewFormId,
        bool? IsHide,
        string? Image,
        int? ViewType,
        int? RegUserId,
        int? regComputerId,
        DateTime RegDate,
        DateTime? CancelDate,
        int ChildrenCount
    );
}