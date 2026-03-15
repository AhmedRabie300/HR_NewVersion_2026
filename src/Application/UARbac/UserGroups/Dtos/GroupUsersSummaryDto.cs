// Application/UARbac/UserGroups/Dtos/GroupUsersSummaryDto.cs
namespace Application.UARbac.UserGroups.Dtos
{
    public sealed record GroupUsersSummaryDto(
        int GroupId,
        string GroupCode,
        string? GroupName,
        int TotalUsers,
        List<UserGroupDto> Users
    );
}