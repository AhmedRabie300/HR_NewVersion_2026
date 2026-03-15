namespace Application.UARbac.ModulePermissions.Dtos
{
    public sealed record UpdateModulePermissionDto(
        int Id,
         int? GroupId,
        int? UserId,
        bool? CanView
    );
}