namespace Application.UARbac.FormPermission.Dtos
{
    public sealed record CreateFormPermissionDto(
        int FormId,
        int? GroupId,
        int? UserId,
        bool? AllowView,
        bool? AllowAdd,
        bool? AllowEdit,
        bool? AllowDelete,
        bool? AllowPrint,
        int? RegUserId,
        int? regComputerId
    );
}