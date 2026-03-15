namespace Application.UARbac.Groups.Dtos
{
    public sealed record UpdateGroupDto(
        int Id,
        string? EngName,
        string? ArbName
    );
}