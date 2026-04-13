using System.Globalization;

namespace API.Middleware
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LanguageMiddleware> _logger;

        public LanguageMiddleware(RequestDelegate next, ILogger<LanguageMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";
            var isPublicEndpoint = path.Contains("/api/auth/login") ||
                                   path.Contains("/api/auth/status") ||
                                   path.Contains("/swagger");

            var lang = GetLanguageFromRequest(context);
            var companyId = GetCompanyIdFromRequest(context);

            if (!isPublicEndpoint && !companyId.HasValue)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "Company ID is required in request header (X-CompanyId or CompanyId)",
                    statusCode = 404
                });
                return;
            }

             int finalLang = lang ?? 1;

            context.Items["Language"] = finalLang;

            if (companyId.HasValue)
                context.Items["CompanyId"] = companyId.Value;

            var culture = finalLang == 2 ? "ar-EG" : "en-US";
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);

            _logger.LogDebug($"Language: {(finalLang == 2 ? "Arabic" : "English")}, CompanyId: {companyId}");

            await _next(context);
        }

        private int? GetLanguageFromRequest(HttpContext context)
        {
             var xLang = context.Request.Headers["X-Lang"].ToString();
            if (!string.IsNullOrEmpty(xLang))
            {
                if (int.TryParse(xLang, out int lang) && (lang == 1 || lang == 2))
                    return lang;
                if (xLang.ToLower() == "ar") return 2;
                if (xLang.ToLower() == "en") return 1;
            }

             var language = context.Request.Headers["X-Language"].ToString();
            if (!string.IsNullOrEmpty(language))
            {
                if (int.TryParse(language, out int lang) && (lang == 1 || lang == 2))
                    return lang;
                if (language.ToLower() == "ar") return 2;
                if (language.ToLower() == "en") return 1;
            }

             var acceptLang = context.Request.Headers["Accept-Language"].ToString();
            if (!string.IsNullOrEmpty(acceptLang))
            {
                var firstLang = acceptLang.Split(',').FirstOrDefault()?.Trim().ToLower();
                if (firstLang != null)
                {
                    if (firstLang.StartsWith("ar")) return 2;
                    if (firstLang.StartsWith("en")) return 1;
                }
            }

             return null;
        }

        private int? GetCompanyIdFromRequest(HttpContext context)
        {
             var xCompanyId = context.Request.Headers["X-CompanyId"].ToString();
            if (!string.IsNullOrEmpty(xCompanyId))
            {
                if (int.TryParse(xCompanyId, out int companyId) && companyId > 0)
                    return companyId;
            }

             var companyIdHeader = context.Request.Headers["CompanyId"].ToString();
            if (!string.IsNullOrEmpty(companyIdHeader))
            {
                if (int.TryParse(companyIdHeader, out int companyId) && companyId > 0)
                    return companyId;
            }

            return null;
        }
    }

    public static class LanguageMiddlewareExtensions
    {
        public static IApplicationBuilder UseLanguageMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LanguageMiddleware>();
        }
    }
}