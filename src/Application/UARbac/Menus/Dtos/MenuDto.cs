namespace Application.UARbac.Menus.Dtos
{
    public sealed record MenuDto(
        int Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? ArbName4S,
        
        List<MenuDto>? Children
    );
}