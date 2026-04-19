using Application.Abstractions;
using Domain.UARbac;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Common.CurrentUser;

public sealed class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
    public int? UserId { get; private set; }
    public int CompanyId { get; private set; }
    public int Language { get; private set; } = 1;
    public bool IsAuthenticated => UserId > 0;


    void ICurrentUserInitializer.Initialize(int userId, int companyId, int language)
    {
        UserId = userId;
        CompanyId = companyId;
        Language = language;
    }


}

internal interface ICurrentUserInitializer
{
    void Initialize(int userId, int companyId, int language);
}
