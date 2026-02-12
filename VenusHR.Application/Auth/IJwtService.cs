using System.Security.Claims;
using VenusHR.Application.Common.DTOs.Login;
using VenusHR.Core.Login;

namespace VenusHR.Application.Common.Interfaces.Auth
{
    public interface IJwtService
    {
        string GenerateToken(Sys_Users user, List<UserGroupDto> groups, List<UserFeatureDto> features);
        ClaimsPrincipal? ValidateToken(string token);
        int? GetUserIdFromToken(string token);
        string? GetUserCodeFromToken(string token);
        bool IsTokenExpired(string token);
    }
}