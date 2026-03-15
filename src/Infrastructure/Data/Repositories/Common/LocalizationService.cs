// Infrastructure/Services/LocalizationService.cs
using Application.Common.Abstractions;
 
namespace Infrastructure.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly Dictionary<string, Dictionary<int, string>> _messages;

        public LocalizationService()
        {
            _messages = new Dictionary<string, Dictionary<int, string>>
            {
                // ===== Validation Messages =====
                ["IdRequired"] = new Dictionary<int, string>
                {
                    { 1, "ID is required" },
                    { 2, "المعرف مطلوب" }
                },
                ["IdGreaterThanZero"] = new Dictionary<int, string>
                {
                    { 1, "ID must be greater than 0" },
                    { 2, "يجب أن يكون المعرف أكبر من 0" }
                },
                ["NameRequired"] = new Dictionary<int, string>
                {
                    { 1, "Name is required" },
                    { 2, "الاسم مطلوب" }
                },
                ["CodeRequired"] = new Dictionary<int, string>
                {
                    { 1, "Code is required" },
                    { 2, "الكود مطلوب" }
                },
                ["MaxLength"] = new Dictionary<int, string>
                {
                    { 1, "Maximum length is {0} characters" },
                    { 2, "الحد الأقصى للطول هو {0} حرف" }
                },

                // ===== CRUD Messages =====
                ["CreatedSuccessfully"] = new Dictionary<int, string>
                {
                    { 1, "{0} created successfully" },
                    { 2, "تم إنشاء {0} بنجاح" }
                },
                ["UpdatedSuccessfully"] = new Dictionary<int, string>
                {
                    { 1, "{0} updated successfully" },
                    { 2, "تم تحديث {0} بنجاح" }
                },
                ["DeletedSuccessfully"] = new Dictionary<int, string>
                {
                    { 1, "{0} deleted successfully" },
                    { 2, "تم حذف {0} بنجاح" }
                },

                ["NotFound"] = new Dictionary<int, string>
                {
                    { 1, "{0} with ID {1} not found" },
                    { 2, "{0} بالمعرف {1} غير موجود" }
                },
                ["UserNotFound"] = new Dictionary<int, string>
                {
                    { 1, "User not found" },
                    { 2, "المستخدم غير موجود" }
                },
                ["GroupNotFound"] = new Dictionary<int, string>
                {
                    { 1, "Group not found" },
                    { 2, "المجموعة غير موجودة" }
                },

                ["Unauthorized"] = new Dictionary<int, string>
                {
                    { 1, "Unauthorized: User not authenticated" },
                    { 2, "غير مصرح: المستخدم غير مسجل الدخول" }
                },
                ["AccessDenied"] = new Dictionary<int, string>
                {
                    { 1, "Access denied. No permissions found." },
                    { 2, "تم رفض الوصول. لا توجد صلاحيات." }
                },
                ["PermissionDenied"] = new Dictionary<int, string>
                {
                    { 1, "You do not have '{0}' permission for this page" },
                    { 2, "ليس لديك صلاحية '{0}' لهذه الصفحة" }
                },

                // ===== Login Messages =====
                ["LoginSuccess"] = new Dictionary<int, string>
                {
                    { 1, "Login successful" },
                    { 2, "تم تسجيل الدخول بنجاح" }
                },
                ["LoginFailed"] = new Dictionary<int, string>
                {
                    { 1, "Login failed" },
                    { 2, "فشل تسجيل الدخول" }
                },
                ["InvalidCredentials"] = new Dictionary<int, string>
                {
                    { 1, "Invalid username or password" },
                    { 2, "اسم المستخدم أو كلمة المرور غير صحيحة" }
                },
                ["AccountInactive"] = new Dictionary<int, string>
                {
                    { 1, "User account is inactive" },
                    { 2, "حساب المستخدم غير مفعل" }
                },
                ["EnterCredentials"] = new Dictionary<int, string>
                {
                    { 1, "Please enter username and password" },
                    { 2, "برجاء ادخال اسم المستخدم وكلمة المرور" }
                },

                // ===== Validation =====
                ["AtLeastOneField"] = new Dictionary<int, string>
                {
                    { 1, "At least one field must be provided to update" },
                    { 2, "يجب توفير حقل واحد على الأقل للتحديث" }
                },
                ["EitherGroupOrUser"] = new Dictionary<int, string>
                {
                    { 1, "Either GroupId or UserId must be provided, but not both" },
                    { 2, "يجب توفير إما GroupId أو UserId، وليس كلاهما" }
                },
                ["AtLeastOnePermission"] = new Dictionary<int, string>
                {
                    { 1, "At least one permission must be granted" },
                    { 2, "يجب منح صلاحية واحدة على الأقل" }
                }
            };
        }

        public string GetMessage(string key, int lang)
        {
            if (_messages.ContainsKey(key) && _messages[key].ContainsKey(lang))
            {
                return _messages[key][lang];
            }

             if (_messages.ContainsKey(key) && _messages[key].ContainsKey(1))
            {
                return _messages[key][1];
            }

            return key; // Return key if nothing found
        }

        public string GetMessage(string key, string lang)
        {
            int langCode = lang?.ToLower() switch
            {
                "en" => 1,
                "ar" => 2,
                _ => 1
            };
            return GetMessage(key, langCode);
        }

        public string this[string key, int lang] => GetMessage(key, lang);
    }
}