// Application/UARbac/UserGroups/Dtos/UpdateUserGroupDto.cs
namespace Application.UARbac.UserGroups.Dtos
{
    public sealed record UpdateUserGroupDto(
        int Id,
        bool? IsPrimary
    );
}