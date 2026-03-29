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

            // تخزين اللغة في HttpContext.Items لاستخدامها في الـ Endpoints
            context.Items["Language"] = lang;

            // تعريب الأرقام والتواريخ حسب اللغة (اختياري)
            var culture = lang == 2 ? "ar-EG" : "en-US";
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);

            _logger.LogDebug($"Language set to: {(lang == 2 ? "Arabic" : "English")}");

            await _next(context);
        }

        private int GetLanguageFromRequest(HttpContext context)
        {
            // 1. قراءة من Header مخصص X-Lang (الأولوية القصوى)
            var xLang = context.Request.Headers["X-Lang"].ToString();
            if (!string.IsNullOrEmpty(xLang))
            {
                if (int.TryParse(xLang, out int lang) && (lang == 1 || lang == 2))
                    return lang;
                if (xLang.ToLower() == "ar") return 2;
                if (xLang.ToLower() == "en") return 1;
            }

            // 2. قراءة من Header مخصص X-Language
            var language = context.Request.Headers["X-Language"].ToString();
            if (!string.IsNullOrEmpty(language))
            {
                if (int.TryParse(language, out int lang) && (lang == 1 || lang == 2))
                    return lang;
                if (language.ToLower() == "ar") return 2;
                if (language.ToLower() == "en") return 1;
            }

            // 3. قراءة من Header Accept-Language (Standard)
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

            // 4. قراءة من Query String (Backup)
            var queryLang = context.Request.Query["lang"].ToString();
            if (!string.IsNullOrEmpty(queryLang))
            {
                if (int.TryParse(queryLang, out int lang) && (lang == 1 || lang == 2))
                    return lang;
                if (queryLang.ToLower() == "ar") return 2;
                if (queryLang.ToLower() == "en") return 1;
            }

            return 1; // Default English
        }
    }

    // Extension method للتسجيل السهل
    public static class LanguageMiddlewareExtensions
    {
        public static IApplicationBuilder UseLanguageMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LanguageMiddleware>();
        }
    }
}