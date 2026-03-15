using Application.UARbac.FormPermission.Dtos;
using Application.UARbac.Groups.Dtos;
using Application.UARbac.Menus.Dtos;
using Application.UARbac.UserGroups.Dtos;

namespace Application.UARbac.Login.Dtos
{
    public sealed record LoginResponseDto(
        bool Success,
        string? Message,
        int? Id,
        string? Code,
        string? EngName,
        string? ArbName,
        string? Token,
        string? DeviceToken,
        List<UserGroupDto>? Groups,
        List<MenuDto>? Menus,
        List<UserFormPermissionDto>? FormPermissions,
        int? EmployeeId,
        string? EmployeeName
    );
}