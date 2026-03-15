
namespace Application.UARbac.UserGroups.Dtos
{
    public sealed record UserGroupDto(
        int Id,
        string Code,
        int UserId,
        string UserCode,
       string? UserName,
        int GroupId,
        string GroupCode,
        string? GroupName
       
    );
}