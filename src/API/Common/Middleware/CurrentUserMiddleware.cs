using Infrastructure.Common;
using Infrastructure.Common.CurrentUser;
using System.Security.Claims;

namespace API.Common.Middleware
{
    public sealed class CurrentUserMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

           
            var initializer = context.RequestServices.GetRequiredService<ICurrentUserInitializer>();
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? "";
            if (path.StartsWith("/swagger") || path.StartsWith("/openapi") || path.StartsWith("/health"))
            {
                await next(context);
                return;
            }


            //var userIdClaim = context.User.FindFirst("user_id")?.Value
            //    ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //if (!int.TryParse(userIdClaim, out var userId))
            //{
            //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    await context.Response.WriteAsync("Invalid token: missing user id");
            //    return;
            //}

            // CompanyId: validate header against claim
            var headerCompanyId = context.Request.Headers["CompanyId"].FirstOrDefault();
            if (!int.TryParse(headerCompanyId, out var requestedCompanyId))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Missing or invalid CompanyId header");
                return;
            }

            // Allowed companies from JWT claim - Need to check if the companyId from header is in the list of allowed companies for the user
            //var allowedCompanies = context.User.FindAll("company_id")
            //    .Select(c => int.TryParse(c.Value, out var v) ? v : 0)
            //    .Where(v => v > 0)
            //    .ToHashSet();

            //if (!allowedCompanies.Contains(requestedCompanyId))
            //{
            //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //    await context.Response.WriteAsync("You do not have access to this company");
            //    return;
            //}

            var headerLang = context.Request.Headers["Language"].FirstOrDefault();
            var language = LanguageResolver.Resolve(headerLang);

            initializer.Initialize(1, requestedCompanyId, language);

            await next(context);
        }
    }
}
