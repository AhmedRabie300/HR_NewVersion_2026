namespace VenusHR.Application.Common.DTOs.Login;

public class LoginRequestDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? DeviceToken { get; set; }
}