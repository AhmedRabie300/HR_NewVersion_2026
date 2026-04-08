// API/Middleware/LanguageMiddleware.cs
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
            var lang = GetLanguageFromRequest(context);
            var companyId = GetCompanyIdFromRequest(context);

            context.Items["Language"] = lang;
            context.Items["CompanyId"] = companyId;

            var culture = lang == 2 ? "ar-EG" : "en-US";
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);

            _logger.LogDebug($"Language: {(lang == 2 ? "Arabic" : "English")}, CompanyId: {companyId}");

            await _next(context);
        }

        private int GetLanguageFromRequest(HttpContext context)
        {
            var xLang = context.Request.Headers["X-Lang"].ToString();
            if (!string.IsNullOrEmpty(xLang))
            {
                if (int.TryParse(xLang, out int lang) && (lang == 1 || lang == 2))
                    return lang;
                if (xLang.ToLower() == "ar") return 2;
                if (xLang.ToLower() == "en") return 1;
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

            return 1;
        }

        private int? GetCompanyIdFromRequest(HttpContext context)
        {
            // X-CompanyId Header
            var xCompanyId = context.Request.Headers["X-CompanyId"].ToString();
            if (!string.IsNullOrEmpty(xCompanyId))
            {
                if (int.TryParse(xCompanyId, out int companyId) && companyId > 0)
                    return companyId;
            }

            // CompanyId Header
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