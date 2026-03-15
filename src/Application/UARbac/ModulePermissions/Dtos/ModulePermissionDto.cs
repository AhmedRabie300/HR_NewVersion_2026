namespace Application.UARbac.ModulePermissions.Dtos
{
    public sealed record ModulePermissionDto(
        int Id,
        int ModuleId,
        string? ModuleCode,
        string? ModuleName,
        int? GroupId,
        string? GroupCode,
        string? GroupName,
        int? UserId,
        string? UserCode,
        string? UserName,
        bool? CanView,
        DateTime RegDate,
        DateTime? CancelDate,
        bool IsActive
    );
}