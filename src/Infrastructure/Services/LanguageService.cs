using Application.Common.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LanguageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetCurrentLanguage()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Items.ContainsKey("Language"))
            {
                return (int)context.Items["Language"]!;
            }
            return 1; // Default English
        }

        public bool IsArabic()
        {
            return GetCurrentLanguage() == 2;
        }

        public bool IsEnglish()
        {
            return GetCurrentLanguage() == 1;
        }
    }
}