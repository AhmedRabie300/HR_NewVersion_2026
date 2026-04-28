namespace Application.Common.Abstractions
{
    public interface ICalendarService
    {
        bool IsHijri(string date);
        bool IsGregorian(string date);
        DateTime GregToHijri(DateTime gregorianDate);
        DateTime HijriToGreg(DateTime hijriDate);
        DateTime GregToHijri(string gregorianDate, string format);
        DateTime HijriToGreg(string hijriDate, string format);
        string FormatHijri(DateTime hijriDate, string format);
        string FormatGreg(DateTime gregorianDate, string format);
        int GetHijriYear(DateTime date);
        int GetGregorianYear(DateTime date);
        string GetHijriMonthName(int month, bool isArabic = true);
        string GetGregorianMonthName(int month, bool isArabic = true);
    }
}