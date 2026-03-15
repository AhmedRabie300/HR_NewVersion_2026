namespace Application.UARbac.UserGroups.Dtos
{
    public sealed record UserGroupsSummaryDto(
        int UserId,
        string UserCode,
        string? UserName,
        int TotalGroups,
        UserGroupDto? PrimaryGroup,
        List<UserGroupDto> Groups
    );
}