using VenusHR.API.Helpers;

public static class JwtMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtMiddleware>();
    }
}