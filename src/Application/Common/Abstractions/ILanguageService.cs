namespace Application.Common.Abstractions
{
    public interface ILanguageService
    {
        int GetCurrentLanguage();
        bool IsArabic();
        bool IsEnglish();
    }
}