
namespace Application.UARbac.FormPermission.Dtos
{
    public sealed record FormPermissionDto(
        int Id,
        int FormId,
        string FormCode,
        string FormName,
        int? GroupId,
        string? GroupName,
        int? UserId,
        string? UserName,
        bool? AllowView,
        bool? AllowAdd,
        bool? AllowEdit,
        bool? AllowDelete,
        bool? AllowPrint,
        DateTime RegDate
    );
}