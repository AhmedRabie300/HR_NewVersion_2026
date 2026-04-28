using System.Globalization;
using Application.Common.Abstractions;

namespace Infrastructure.Services.Common
{
    public sealed class CalendarService : ICalendarService
    {
        private readonly GregorianCalendar _gregorianCalendar;
        private readonly UmAlQuraCalendar _hijriCalendar;

        public CalendarService()
        {
            _gregorianCalendar = new GregorianCalendar(GregorianCalendarTypes.Localized);
            _hijriCalendar = new UmAlQuraCalendar();
        }

        public bool IsHijri(string date)
        {
            if (string.IsNullOrWhiteSpace(date)) return false;
            return date.Contains("هـ") || date.Contains("هجري") || date.Contains("Hijri");
        }

        public bool IsGregorian(string date)
        {
            if (string.IsNullOrWhiteSpace(date)) return false;
            return date.Contains("م") || date.Contains("ميلادي") || date.Contains("Gregorian");
        }

        public DateTime GregToHijri(DateTime gregorianDate)
        {
            var hijriYear = _hijriCalendar.GetYear(gregorianDate);
            var hijriMonth = _hijriCalendar.GetMonth(gregorianDate);
            var hijriDay = _hijriCalendar.GetDayOfMonth(gregorianDate);
            return new DateTime(hijriYear, hijriMonth, hijriDay);
        }

        public DateTime HijriToGreg(DateTime hijriDate)
        {
            var gregorianYear = _gregorianCalendar.GetYear(hijriDate);
            var gregorianMonth = _gregorianCalendar.GetMonth(hijriDate);
            var gregorianDay = _gregorianCalendar.GetDayOfMonth(hijriDate);
            return new DateTime(gregorianYear, gregorianMonth, gregorianDay);
        }

        public DateTime GregToHijri(string gregorianDate, string format)
        {
            if (!DateTime.TryParseExact(gregorianDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                throw new Exception($"Invalid date format: {gregorianDate}");
            return GregToHijri(date);
        }

        public DateTime HijriToGreg(string hijriDate, string format)
        {
            if (!DateTime.TryParseExact(hijriDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                throw new Exception($"Invalid date format: {hijriDate}");
            return HijriToGreg(date);
        }

        public string FormatHijri(DateTime hijriDate, string format)
        {
            return hijriDate.ToString(format, CultureInfo.InvariantCulture);
        }

        public string FormatGreg(DateTime gregorianDate, string format)
        {
            return gregorianDate.ToString(format, CultureInfo.InvariantCulture);
        }

        public int GetHijriYear(DateTime date)
        {
            return _hijriCalendar.GetYear(date);
        }

        public int GetGregorianYear(DateTime date)
        {
            return date.Year;
        }

        public string GetHijriMonthName(int month, bool isArabic = true)
        {
            var monthNames = isArabic ? new[]
            {
                "", "محرم", "صفر", "ربيع الأول", "ربيع الآخر", "جمادى الأولى", "جمادى الآخرة",
                "رجب", "شعبان", "رمضان", "شوال", "ذو القعدة", "ذو الحجة"
            } : new[]
            {
                "", "Muharram", "Safar", "Rabi' al-awwal", "Rabi' al-thani", "Jumada al-awwal", "Jumada al-thani",
                "Rajab", "Sha'ban", "Ramadan", "Shawwal", "Dhu al-Qi'dah", "Dhu al-Hijjah"
            };
            return month >= 1 && month <= 12 ? monthNames[month] : "";
        }

        public string GetGregorianMonthName(int month, bool isArabic = true)
        {
            var monthNames = isArabic ? new[]
            {
                "", "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو",
                "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر"
            } : new[]
            {
                "", "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            };
            return month >= 1 && month <= 12 ? monthNames[month] : "";
        }
    }
}