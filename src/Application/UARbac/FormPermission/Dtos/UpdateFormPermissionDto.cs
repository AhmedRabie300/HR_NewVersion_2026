namespace Application.UARbac.FormPermission.Dtos
{
    public sealed record UpdateFormPermissionDto(
        int Id,
        bool? AllowView,
        bool? AllowAdd,
        bool? AllowEdit,
        bool? AllowDelete,
        bool? AllowPrint
    );
}