namespace Application.UARbac.Menus.Dtos
{
    public sealed record MenuTreeDto(
        int Id,
        string? Code,
        string? Name,
        int? ParentId,
        int? Rank,
        string? Icon,
        string? Route,
        List<MenuTreeDto>? Children
    );
}