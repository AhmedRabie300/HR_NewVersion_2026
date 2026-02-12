// CreateUserDto.cs
namespace VenusHR.Application.Common.DTOs.Users;

public class CreateUserDto
{
    public string Code { get; set; } = null!;
    public string? EngName { get; set; }
    public string? ArbName { get; set; }
    public string? Password { get; set; }
    public bool? IsAdmin { get; set; }
    public string? DeviceToken { get; set; }
    public List<int> GroupIds { get; set; } = new();
}

 
// UserDto.cs
public class UserDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string? EngName { get; set; }
    public string? ArbName { get; set; }
    public bool? IsAdmin { get; set; }
    public bool? IsActive { get; set; }
    public DateTime RegDate { get; set; }
    public string? DeviceToken { get; set; }
    public List<UserGroupInfoDto> Groups { get; set; } = new();
}

