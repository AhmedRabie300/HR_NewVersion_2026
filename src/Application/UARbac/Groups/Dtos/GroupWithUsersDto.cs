namespace Application.UARbac.Groups.Dtos
{
    public sealed record GroupWithUsersDto(
        int Id,
        string Code,
        string? EngName,
        string? ArbName,
        DateTime RegDate,
        DateTime? CancelDate,
        int UsersCount,
        List<GroupUserDto> Users
    );

    public sealed record GroupUserDto(
        int UserId,
        string UserCode,
        string? UserName
       
    );
}