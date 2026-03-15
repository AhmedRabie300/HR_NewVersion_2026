// Application/UARbac/Groups/Dtos/CreateGroupDto.cs
namespace Application.UARbac.Groups.Dtos
{
    public sealed record CreateGroupDto(
        string Code,
        string? EngName,
        string? ArbName
    );
}