using Application.Common.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class ContextService : IContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextService(IHttpContextAccessor httpContextAccessor)
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
        public int GetCurrentCompanyId()
        {
            var context = _httpContextAccessor.HttpContext;
            var companyId = context?.Items["CompanyId"] as int?;
            return companyId ?? 0;
        }
        public bool HasCompanyId()
        {
            var context = _httpContextAccessor.HttpContext;
            return context?.Items.ContainsKey("CompanyId") == true && context.Items["CompanyId"] != null;
        }
    }
}