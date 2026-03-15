namespace Application.UARbac.ModulePermissions.Dtos
{
    public sealed record CreateModulePermissionDto(
        int ModuleId,
        int? GroupId,
        int? UserId,
        bool? CanView,
        int? RegUserId,
        string? RegComputerId
    );
}