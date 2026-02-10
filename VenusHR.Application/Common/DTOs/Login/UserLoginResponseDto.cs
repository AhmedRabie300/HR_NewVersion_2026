namespace VenusHR.Application.Common.DTOs.Login;

public class UserLoginResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public UserDataDto? Data { get; set; }
}