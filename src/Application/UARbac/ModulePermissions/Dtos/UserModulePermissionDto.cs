namespace Application.UARbac.ModulePermissions.Dtos
{
    public sealed record UserModulePermissionDto(
        int ModuleId,
        string ModuleCode,
        string ModuleName,
        bool CanView,
        string PermissionSource  
    );
}