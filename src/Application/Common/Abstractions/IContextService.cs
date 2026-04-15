namespace Application.Common.Abstractions
{
    public interface IContextService
    {
        int GetCurrentLanguage();
        bool IsArabic();
        bool IsEnglish();
        int GetCurrentCompanyId();
        bool HasCompanyId();
        int? GetCurrentUserId();

    }
}