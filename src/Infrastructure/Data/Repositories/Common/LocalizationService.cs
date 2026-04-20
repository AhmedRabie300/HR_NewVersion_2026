// Infrastructure/Services/LocalizationService.cs
using Application.Common.Abstractions;

namespace Infrastructure.Services;

public class LocalizationService : ILocalizationService
{
    private readonly Dictionary<string, Dictionary<int, string>> _messages;

    public LocalizationService()
    {
        _messages = new Dictionary<string, Dictionary<int, string>>();

        RegisterValidation();
        RegisterCrud();
        RegisterAuth();
        RegisterEntities();
        RegisterFieldMessages();
    }

    public string GetMessage(string key, int lang)
    {
        if (_messages.TryGetValue(key, out var translations))
        {
            if (translations.TryGetValue(lang, out var message))
                return message;

            if (translations.TryGetValue(1, out var fallback))
                return fallback;
        }

        return key; // return the key itself as last resort
    }

    public string GetMessage(string key, string lang)
    {
        var langCode = lang?.ToLowerInvariant() switch
        {
            "ar" => 2,
            _ => 1
        };
        return GetMessage(key, langCode);
    }

    public string this[string key, int lang] => GetMessage(key, lang);

    // ─────────────── Private Registration Methods ───────────────

    private void Add(string key, string en, string ar)
    {
        _messages[key] = new Dictionary<int, string>
        {
            { 1, en },
            { 2, ar }
        };
    }

    private void RegisterValidation()
    {
        Add("IdRequired", "ID is required", "المعرف مطلوب");
        Add("IdGreaterThanZero", "ID must be greater than 0", "يجب أن يكون المعرف أكبر من 0");
        Add("NameRequired", "Name is required", "الاسم مطلوب");
        Add("CodeRequired", "Code is required", "الكود مطلوب");
        Add("PasswordRequired", "Password is required", "كلمة المرور مطلوبة");
        Add("MaxLength", "Maximum length is {0} characters", "الحد الأقصى للطول هو {0} حرف");
        Add("CodePattern", "Code can only contain letters, numbers and underscore", "الكود يمكن أن يحتوي فقط على أحرف وأرقام وشرطة سفلية");
        Add("FormIdGreaterThanZero", "Form ID must be greater than 0", "معرف النموذج يجب أن يكون أكبر من 0");
        Add("RankMustBePositive", "Rank must be zero or positive number", "الترتيب يجب أن يكون صفر أو رقم موجب");
        Add("UserIdRequired", "User ID is required", "معرف المستخدم مطلوب");
        Add("GroupIdRequired", "Group ID is required", "معرف المجموعة مطلوب");
        Add("UserAlreadyInGroup", "{0} already belongs to this {1}", "{0} موجود بالفعل في هذه {1}");
        Add("ModuleIdRequired", "Module ID is required", "معرف الوحدة مطلوب");
        Add("CanViewMustBeTrue", "CanView must be true if provided", "يجب أن تكون صلاحية العرض true إذا تم توفيرها");
        Add("AtLeastOneField", "At least one field must be provided to update", "يجب توفير حقل واحد على الأقل للتحديث");
        Add("EitherGroupOrUser", "Either GroupId or UserId must be provided, but not both", "يجب توفير إما GroupId أو UserId، وليس كلاهما");
        Add("AtLeastOnePermission", "At least one permission must be granted", "يجب منح صلاحية واحدة على الأقل");
        Add("MenuCannotBeParentOfItself", "Menu cannot be parent of itself", "القائمة لا يمكن أن تكون أباً لنفسها");
    }

    private void RegisterCrud()
    {
        Add("CreatedSuccessfully", "{0} created successfully", "تم إنشاء {0} بنجاح");
        Add("UpdatedSuccessfully", "{0} updated successfully", "تم تحديث {0} بنجاح");
        Add("DeletedSuccessfully", "{0} deleted successfully", "تم حذف {0} بنجاح");
        Add("CodeExists", "{0} with code '{1}' already exists", "{0} بالكود '{1}' موجود بالفعل");
        Add("NotFound", "{0} with ID '{1}' was not found", "{0} بالمعرف '{1}' غير موجود");
        Add("PermissionAlreadyExists", "Permission already exists for this {0} and {1}", "الصلاحية موجودة بالفعل لهذا {0} و {1}");
        Add("CannotDeleteHasChildren", "Cannot delete {0} because it has child items", "لا يمكن حذف {0} لأنه يحتوي على عناصر فرعية");
    }

    private void RegisterAuth()
    {
        Add("Unauthorized", "Unauthorized: User not authenticated", "غير مصرح: المستخدم غير مسجل الدخول");
        Add("AccessDenied", "Access denied. No permissions found.", "تم رفض الوصول. لا توجد صلاحيات.");
        Add("PermissionDenied", "You do not have '{0}' permission for this page", "ليس لديك صلاحية '{0}' لهذه الصفحة");
        Add("LoginSuccess", "Login successful", "تم تسجيل الدخول بنجاح");
        Add("LoginFailed", "Login failed", "فشل تسجيل الدخول");
        Add("InvalidCredentials", "Invalid username or password", "اسم المستخدم أو كلمة المرور غير صحيحة");
        Add("AccountInactive", "User account is inactive", "حساب المستخدم غير مفعل");
        Add("EnterCredentials", "Please enter username and password", "برجاء ادخال اسم المستخدم وكلمة المرور");
    }

    private void RegisterEntities()
    {
        Add("Company", "Company", "الشركة");
        Add("Branch", "Branch", "الفرع");
        Add("ParentBranch", "Parent branch", "الفرع الأب");
        Add("Department", "Department", "القسم");
        Add("ParentDepartment", "Parent department", "القسم الأب");
        Add("Sector", "Sector", "القطاع");
        Add("ParentSector", "Parent sector", "القطاع الأب");
        Add("Location", "Location", "الموقع");
        Add("ParentLocation", "Parent location", "الموقع الأب");
        Add("Position", "Position", "الوظيفة");
        Add("ParentPosition", "Parent position", "الوظيفة الأب");
        Add("Sponsor", "Sponsor", "الكفيل");
        Add("Currency", "Currency", "العملة");
        Add("Bank", "Bank", "البنك");
        Add("Education", "Education", "المؤهل العلمي");
        Add("Profession", "Profession", "المهنة");
        Add("ContractType", "Contract type", "نوع العقد");
        Add("DependantType", "Dependant type", "نوع المعال");
        Add("Nationality", "Nationality", "الجنسية");
        Add("Religion", "Religion", "الديانة");
        Add("BloodGroup", "Blood group", "فصيلة الدم");
        Add("MaritalStatus", "Marital status", "الحالة الاجتماعية");
        Add("Document", "Document", "المستند");
        Add("DocumentTypesGroup", "Document types group", "مجموعة أنواع المستندات");
        Add("Module", "Module", "الوحدة");
        Add("ModulePermission", "Module permission", "صلاحية الوحدة");
        Add("Form", "Form", "النموذج");
        Add("FormPermission", "Form permission", "صلاحية النموذج");
        Add("Menu", "Menu", "القائمة");
        Add("ParentMenu", "Parent menu", "القائمة الأب");
        Add("Group", "Group", "المجموعة");
        Add("User", "User", "المستخدم");
        Add("UserGroup", "User group", "مجموعة المستخدمين");
        Add("Country", "Country", "الدولة");
        Add("City", "City", "المدينة");
        Add("VacationsType", "Vacations Type", "نوع الإجازة");
        Add("Gender", "Gender", "النوع");
        Add("TransactionsGroup", "Transactions Group", "مجموعة المعاملات");
        Add("Region", "Region", "المنطقة");
    }

    private void RegisterFieldMessages()
    {
        Add("ParentBranchRequired", "Parent branch ID must be greater than 0", "معرف الفرع الأب يجب أن يكون أكبر من 0");
        Add("CountryRequired", "Country ID must be greater than 0", "معرف الدولة يجب أن يكون أكبر من 0");
        Add("CityRequired", "City ID must be greater than 0", "معرف المدينة يجب أن يكون أكبر من 0");
        Add("RegionRequired", "Region ID must be greater than 0", "معرف المنطقة يجب أن يكون أكبر من 0");
        Add("CurrencyRequired", "Currency ID must be greater than 0", "معرف العملة يجب أن يكون أكبر من 0");
        Add("NationalityRequired", "Nationality ID must be greater than 0", "معرف الجنسية يجب أن يكون أكبر من 0");
        Add("CapitalRequired", "Capital ID must be greater than 0", "معرف العاصمة يجب أن يكون أكبر من 0");
        Add("CompanyRequired", "Company is required", "الشركة مطلوبة");
        Add("SequenceLengthMustBePositive", "Sequence length must be greater than 0", "طول التسلسل يجب أن يكون أكبر من 0");
        Add("PrefixMustBePositive", "Prefix must be greater than 0", "البادئة يجب أن تكون أكبر من 0");
        Add("SeparatorMaxLength", "Separator must be a single character", "العلامة الفاصلة يجب أن تكون حرف واحد");
        Add("PrepareDayRange", "Prepare day must be between 1 and 31", "يوم التحضير يجب أن يكون بين 1 و 31");
        Add("ExecuseRequestHoursPositive", "Execuse request hours allowed must be greater than 0", "ساعات الإذن المسموحة يجب أن تكون أكبر من 0");
        Add("AtLeastOneIdentifier", "At least one identifier (Company or Branch) must be provided", "يجب توفير معرّف واحد على الأقل (شركة أو فرع)");
        Add("ParentMustBeSameCompany", "Parent must belong to the same company", "الأب يجب أن يكون تابع لنفس الشركة");
        Add("SponsorNumberMustBePositive", "Sponsor number must be greater than 0", "رقم الكفيل يجب أن يكون أكبر من 0");
        Add("IsSpecialRequired", "Special contract flag is required", "علامة العقد الخاص مطلوبة");
        Add("DocumentTypesGroupRequired", "Document types group is required", "مجموعة أنواع المستندات مطلوبة");
        Add("DecimalFractionRequired", "Decimal fraction is required", "الجزء العشري مطلوب");
        Add("TravelRouteMustBePositive", "Travel route must be greater than 0", "مسار السفر يجب أن يكون أكبر من 0");
        Add("TravelClassMustBePositive", "Travel class must be greater than 0", "درجة السفر يجب أن تكون أكبر من 0");
        Add("TicketAmountMustBePositive", "Ticket amount must be greater than 0", "قيمة التذكرة يجب أن تكون أكبر من 0");
        Add("SexMustBeMOrFOrB", "Sex must be 'M' (Male), 'F' (Female), or 'B' (Both)", "النوع يجب أن يكون 'M' (ذكر)، 'F' (أنثى)، أو 'B' (كلاهما)");
        Add("TimesNoInYearGreaterThanZero", "Times number in year must be greater than 0", "عدد المرات في السنة يجب أن يكون أكبر من 0");
        Add("AllowedDaysNoGreaterThanZero", "Allowed days number must be greater than 0", "عدد الأيام المسموحة يجب أن يكون أكبر من 0");
        Add("PercentageBetween0And100", "Percentage must be between 0 and 100", "النسبة المئوية يجب أن تكون بين 0 و 100");
        Add("EngNameAlreadyExists", "English name '{0}' already exists", "الاسم الإنجليزي '{0}' موجود بالفعل");
        Add("ArbNameAlreadyExists", "Arabic name '{0}' already exists", "الاسم العربي '{0}' موجود بالفعل");
    }
}