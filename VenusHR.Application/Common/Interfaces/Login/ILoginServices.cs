using VenusHR.Application.Common.DTOs.Login;

namespace VenusHR.Application.Common.Interfaces.Login;

public interface ILoginServices
{
     Task<UserLoginResponseDto> LoginAsync(LoginRequestDto request, int lang);

     object Login(string username, string password, int lang, string deviceToken);
}