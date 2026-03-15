// Application/UARbac/Permissions/Dtos/UserFormPermissionDto.cs
namespace Application.UARbac.FormPermission.Dtos
{
    public sealed record UserFormPermissionDto(
        int FormId,
        string FormCode,
        string FormName,
        bool AllowView,
        bool AllowAdd,
        bool AllowEdit,
        bool AllowDelete,
        bool AllowPrint,
        string PermissionSource // "User", "Group", or "Inherited"
    );
}