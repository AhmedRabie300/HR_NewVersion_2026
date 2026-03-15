using Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Common.CurrentUser;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
                return null;

            var claimValue = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(claimValue, out var id))
                return id;

            return null;
        }
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
}
