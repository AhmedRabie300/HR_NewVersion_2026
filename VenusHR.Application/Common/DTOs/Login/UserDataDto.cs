namespace VenusHR.Application.Common.DTOs.Login;

public class UserDataDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string? EngName { get; set; }
    public string? ArbName { get; set; }
    public string UserName => Code;
    public bool? IsAdmin { get; set; }
    public bool? IsClient { get; set; }
    public string Token { get; set; } = null!;
    public string? DeviceToken { get; set; }

    // Groups
    public List<UserGroupDto> Groups { get; set; } = new();

    // Features
    public List<UserFeatureDto> Features { get; set; } = new();
}