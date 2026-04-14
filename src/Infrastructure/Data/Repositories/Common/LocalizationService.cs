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
                ["PasswordRequired"] = new Dictionary<int, string>
                {
                    { 1, "Password is required" },
                    { 2, "كلمة المرور مطلوبة" }
                },
                ["MaxLength"] = new Dictionary<int, string>
                {
                    { 1, "Maximum length is {0} characters" },
                    { 2, "الحد الأقصى للطول هو {0} حرف" }
                },
                ["CodePattern"] = new Dictionary<int, string>
                {
                    { 1, "Code can only contain letters, numbers and underscore" },
                    { 2, "الكود يمكن أن يحتوي فقط على أحرف وأرقام وشرطة سفلية" }
                },
                ["FormIdGreaterThanZero"] = new Dictionary<int, string>
                {
                    { 1, "Form ID must be greater than 0" },
                    { 2, "معرف النموذج يجب أن يكون أكبر من 0" }
                },
                ["RankMustBePositive"] = new Dictionary<int, string>
                {
                    { 1, "Rank must be zero or positive number" },
                    { 2, "الترتيب يجب أن يكون صفر أو رقم موجب" }
                },
                ["UserIdRequired"] = new Dictionary<int, string>
                {
                    { 1, "User ID is required" },
                    { 2, "معرف المستخدم مطلوب" }
                },
                ["GroupIdRequired"] = new Dictionary<int, string>
                {
                    { 1, "Group ID is required" },
                    { 2, "معرف المجموعة مطلوب" }
                },
                ["UserAlreadyInGroup"] = new Dictionary<int, string>
                {
                    { 1, "{0} already belongs to this {1}" },
                    { 2, "{0} موجود بالفعل في هذه {1}" }
                },
                ["ModuleIdRequired"] = new Dictionary<int, string>
                {
                    { 1, "Module ID is required" },
                    { 2, "معرف الوحدة مطلوب" }
                },
                ["CanViewMustBeTrue"] = new Dictionary<int, string>
                {
                    { 1, "CanView must be true if provided" },
                    { 2, "يجب أن تكون صلاحية العرض true إذا تم توفيرها" }
                },
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
                },
                ["MenuCannotBeParentOfItself"] = new Dictionary<int, string>
                {
                    { 1, "Menu cannot be parent of itself" },
                    { 2, "القائمة لا يمكن أن تكون أباً لنفسها" }
                },
                ["ParentMenu"] = new Dictionary<int, string>
                {
                    { 1, "Parent menu" },
                    { 2, "القائمة الأب" }
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
            
                ["CodeExists"] = new Dictionary<int, string>
                {
                    { 1, "{0} with code '{1}' already exists" },
                    { 2, "{0} بالكود '{1}' موجود بالفعل" }
                },
                ["PermissionAlreadyExists"] = new Dictionary<int, string>
                {
                    { 1, "Permission already exists for this {0} and {1}" },
                    { 2, "الصلاحية موجودة بالفعل لهذا {0} و {1}" }
                },
                ["CannotDeleteHasChildren"] = new Dictionary<int, string>
                {
                    { 1, "Cannot delete {0} because it has child items" },
                    { 2, "لا يمكن حذف {0} لأنه يحتوي على عناصر فرعية" }
                },
                 ["City"] = new Dictionary<int, string>
{
    { 1, "City" },
    { 2, "المدينة" }
},
                ["RegionRequired"] = new Dictionary<int, string>
{
    { 1, "Region ID must be greater than 0" },
    { 2, "معرف المنطقة يجب أن يكون أكبر من 0" }
},
                // ===== Authentication Messages =====
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

                // ===== Entity Names =====
                ["Company"] = new Dictionary<int, string>
                {
                    { 1, "Company" },
                    { 2, "الشركة" }
                },
                ["Branch"] = new Dictionary<int, string>
                {
                    { 1, "Branch" },
                    { 2, "الفرع" }
                },
                ["ParentBranch"] = new Dictionary<int, string>
                {
                    { 1, "Parent branch" },
                    { 2, "الفرع الأب" }
                },
                ["Department"] = new Dictionary<int, string>
                {
                    { 1, "Department" },
                    { 2, "القسم" }
                },
                ["ParentDepartment"] = new Dictionary<int, string>
                {
                    { 1, "Parent department" },
                    { 2, "القسم الأب" }
                },
                ["Sector"] = new Dictionary<int, string>
                {
                    { 1, "Sector" },
                    { 2, "القطاع" }
                },
                ["ParentSector"] = new Dictionary<int, string>
                {
                    { 1, "Parent sector" },
                    { 2, "القطاع الأب" }
                },
                ["Location"] = new Dictionary<int, string>
                {
                    { 1, "Location" },
                    { 2, "الموقع" }
                },
                ["ParentLocation"] = new Dictionary<int, string>
                {
                    { 1, "Parent location" },
                    { 2, "الموقع الأب" }
                },
                ["Position"] = new Dictionary<int, string>
                {
                    { 1, "Position" },
                    { 2, "الوظيفة" }
                },
                ["ParentPosition"] = new Dictionary<int, string>
                {
                    { 1, "Parent position" },
                    { 2, "الوظيفة الأب" }
                },
                ["Sponsor"] = new Dictionary<int, string>
                {
                    { 1, "Sponsor" },
                    { 2, "الكفيل" }
                },
                ["Currency"] = new Dictionary<int, string>
                {
                    { 1, "Currency" },
                    { 2, "العملة" }
                },
                ["Bank"] = new Dictionary<int, string>
                {
                    { 1, "Bank" },
                    { 2, "البنك" }
                },
                ["Education"] = new Dictionary<int, string>
                {
                    { 1, "Education" },
                    { 2, "المؤهل العلمي" }
                },
                ["Profession"] = new Dictionary<int, string>
                {
                    { 1, "Profession" },
                    { 2, "المهنة" }
                },
                ["ContractType"] = new Dictionary<int, string>
                {
                    { 1, "Contract type" },
                    { 2, "نوع العقد" }
                },
                ["DependantType"] = new Dictionary<int, string>
                {
                    { 1, "Dependant type" },
                    { 2, "نوع المعال" }
                },
                ["Nationality"] = new Dictionary<int, string>
                {
                    { 1, "Nationality" },
                    { 2, "الجنسية" }
                },
                ["Religion"] = new Dictionary<int, string>
                {
                    { 1, "Religion" },
                    { 2, "الديانة" }
                },
                ["BloodGroup"] = new Dictionary<int, string>
                {
                    { 1, "Blood group" },
                    { 2, "فصيلة الدم" }
                },
                ["MaritalStatus"] = new Dictionary<int, string>
                {
                    { 1, "Marital status" },
                    { 2, "الحالة الاجتماعية" }
                },
                ["Document"] = new Dictionary<int, string>
                {
                    { 1, "Document" },
                    { 2, "المستند" }
                },
                ["DocumentTypesGroup"] = new Dictionary<int, string>
                {
                    { 1, "Document types group" },
                    { 2, "مجموعة أنواع المستندات" }
                },
                ["Module"] = new Dictionary<int, string>
                {
                    { 1, "Module" },
                    { 2, "الوحدة" }
                },
                ["ModulePermission"] = new Dictionary<int, string>
                {
                    { 1, "Module permission" },
                    { 2, "صلاحية الوحدة" }
                },
                ["Form"] = new Dictionary<int, string>
                {
                    { 1, "Form" },
                    { 2, "النموذج" }
                },
                ["FormPermission"] = new Dictionary<int, string>
                {
                    { 1, "Form permission" },
                    { 2, "صلاحية النموذج" }
                },
                ["Menu"] = new Dictionary<int, string>
                {
                    { 1, "Menu" },
                    { 2, "القائمة" }
                },
                ["Group"] = new Dictionary<int, string>
                {
                    { 1, "Group" },
                    { 2, "المجموعة" }
                },
                ["User"] = new Dictionary<int, string>
                {
                    { 1, "User" },
                    { 2, "المستخدم" }
                },
                ["UserGroup"] = new Dictionary<int, string>
                {
                    { 1, "User group" },
                    { 2, "مجموعة المستخدمين" }
                },
 
                ["Country"] = new Dictionary<int, string>
{
    { 1, "Country" },
    { 2, "الدولة" }
},

                ["CurrencyRequired"] = new Dictionary<int, string>
{
    { 1, "Currency ID must be greater than 0" },
    { 2, "معرف العملة يجب أن يكون أكبر من 0" }
},

                ["NationalityRequired"] = new Dictionary<int, string>
{
    { 1, "Nationality ID must be greater than 0" },
    { 2, "معرف الجنسية يجب أن يكون أكبر من 0" }
},

                ["CapitalRequired"] = new Dictionary<int, string>
{
    { 1, "Capital ID must be greater than 0" },
    { 2, "معرف العاصمة يجب أن يكون أكبر من 0" }
},

                ["CodeExists"] = new Dictionary<int, string>
{
    { 1, "{0} with code '{1}' already exists" },
    { 2, "{0} بالكود '{1}' موجود بالفعل" }
},
                // Infrastructure/Services/LocalizationService.cs

                ["Bank"] = new Dictionary<int, string>
{
    { 1, "Bank" },
    { 2, "البنك" }
},

                ["CodeExists"] = new Dictionary<int, string>
{
    { 1, "{0} with code '{1}' already exists" },
    { 2, "{0} بالكود '{1}' موجود بالفعل" }
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

                ["IdGreaterThanZero"] = new Dictionary<int, string>
{
    { 1, "ID must be greater than 0" },
    { 2, "المعرف يجب أن يكون أكبر من 0" }
},

                ["AtLeastOneField"] = new Dictionary<int, string>
{
    { 1, "At least one field must be provided for update" },
    { 2, "يجب توفير حقل واحد على الأقل للتحديث" }
},

 

                // ===== Specific Field Messages =====
                ["ParentBranchRequired"] = new Dictionary<int, string>
                {
                    { 1, "Parent branch ID must be greater than 0" },
                    { 2, "معرف الفرع الأب يجب أن يكون أكبر من 0" }
                },
                ["CountryRequired"] = new Dictionary<int, string>
                {
                    { 1, "Country ID must be greater than 0" },
                    { 2, "معرف الدولة يجب أن يكون أكبر من 0" }
                },
                ["CityRequired"] = new Dictionary<int, string>
                {
                    { 1, "City ID must be greater than 0" },
                    { 2, "معرف المدينة يجب أن يكون أكبر من 0" }
                },
                ["SequenceLengthMustBePositive"] = new Dictionary<int, string>
                {
                    { 1, "Sequence length must be greater than 0" },
                    { 2, "طول التسلسل يجب أن يكون أكبر من 0" }
                },
                ["PrefixMustBePositive"] = new Dictionary<int, string>
                {
                    { 1, "Prefix must be greater than 0" },
                    { 2, "البادئة يجب أن تكون أكبر من 0" }
                },
                ["SeparatorMaxLength"] = new Dictionary<int, string>
                {
                    { 1, "Separator must be a single character" },
                    { 2, "العلامة الفاصلة يجب أن تكون حرف واحد" }
                },
                ["PrepareDayRange"] = new Dictionary<int, string>
                {
                    { 1, "Prepare day must be between 1 and 31" },
                    { 2, "يوم التحضير يجب أن يكون بين 1 و 31" }
                },
                ["ExecuseRequestHoursPositive"] = new Dictionary<int, string>
                {
                    { 1, "Execuse request hours allowed must be greater than 0" },
                    { 2, "ساعات الإذن المسموحة يجب أن تكون أكبر من 0" }
                },
                ["AtLeastOneIdentifier"] = new Dictionary<int, string>
                {
                    { 1, "At least one identifier (Company or Branch) must be provided" },
                    { 2, "يجب توفير معرّف واحد على الأقل (شركة أو فرع)" }
                },
                ["ParentMustBeSameCompany"] = new Dictionary<int, string>
                {
                    { 1, "Parent must belong to the same company" },
                    { 2, "الأب يجب أن يكون تابع لنفس الشركة" }
                },
                ["SponsorNumberMustBePositive"] = new Dictionary<int, string>
                {
                    { 1, "Sponsor number must be greater than 0" },
                    { 2, "رقم الكفيل يجب أن يكون أكبر من 0" }
                },
                ["IsSpecialRequired"] = new Dictionary<int, string>
                {
                    { 1, "Special contract flag is required" },
                    { 2, "علامة العقد الخاص مطلوبة" }
                },
                ["DocumentTypesGroupRequired"] = new Dictionary<int, string>
                {
                    { 1, "Document types group is required" },
                    { 2, "مجموعة أنواع المستندات مطلوبة" }
                },
                ["DecimalFractionRequired"] = new Dictionary<int, string>
                {
                    { 1, "Decimal fraction is required" },
                    { 2, "الجزء العشري مطلوب" }
                },
                ["CompanyRequired"] = new Dictionary<int, string>
                {
                    { 1, "Company is required" },
                    { 2, "الشركة مطلوبة" }
                },
                ["TravelRouteMustBePositive"] = new Dictionary<int, string>
                {
                    { 1, "Travel route must be greater than 0" },
                    { 2, "مسار السفر يجب أن يكون أكبر من 0" }
                },
                ["TravelClassMustBePositive"] = new Dictionary<int, string>
                {
                    { 1, "Travel class must be greater than 0" },
                    { 2, "درجة السفر يجب أن تكون أكبر من 0" }
                },
                ["TicketAmountMustBePositive"] = new Dictionary<int, string>
                {
                    { 1, "Ticket amount must be greater than 0" },
                    { 2, "قيمة التذكرة يجب أن تكون أكبر من 0" }
                },
 
                ["VacationsType"] = new Dictionary<int, string>
{
    { 1, "Vacations Type" },
    { 2, "نوع الإجازة" }
},

                ["CodeExists"] = new Dictionary<int, string>
{
    { 1, "{0} with code '{1}' already exists" },
    { 2, "{0} بالكود '{1}' موجود بالفعل" }
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

                ["IdGreaterThanZero"] = new Dictionary<int, string>
{
    { 1, "ID must be greater than 0" },
    { 2, "المعرف يجب أن يكون أكبر من 0" }
},

                ["AtLeastOneField"] = new Dictionary<int, string>
{
    { 1, "At least one field must be provided for update" },
    { 2, "يجب توفير حقل واحد على الأقل للتحديث" }
},

                ["NotFound"] = new Dictionary<int, string>
{
    { 1, "{0} with ID '{1}' was not found" },
    { 2, "{0} بالمعرف '{1}' غير موجود" }
},

 
                ["SexMustBeMOrFOrB"] = new Dictionary<int, string>
{
    { 1, "Sex must be 'M' (Male), 'F' (Female), or 'B' (Both)" },
    { 2, "النوع يجب أن يكون 'M' (ذكر)، 'F' (أنثى)، أو 'B' (كلاهما)" }
},

                ["TimesNoInYearGreaterThanZero"] = new Dictionary<int, string>
{
    { 1, "Times number in year must be greater than 0" },
    { 2, "عدد المرات في السنة يجب أن يكون أكبر من 0" }
},

                ["AllowedDaysNoGreaterThanZero"] = new Dictionary<int, string>
{
    { 1, "Allowed days number must be greater than 0" },
    { 2, "عدد الأيام المسموحة يجب أن يكون أكبر من 0" }
},

                ["PercentageBetween0And100"] = new Dictionary<int, string>
{
    { 1, "Percentage must be between 0 and 100" },
    { 2, "النسبة المئوية يجب أن تكون بين 0 و 100" }
},
                // Infrastructure/Services/LocalizationService.cs

                ["Gender"] = new Dictionary<int, string>
{
    { 1, "Gender" },
    { 2, "النوع" }
},

                ["CodeExists"] = new Dictionary<int, string>
{
    { 1, "{0} with code '{1}' already exists" },
    { 2, "{0} بالكود '{1}' موجود بالفعل" }
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

                ["IdGreaterThanZero"] = new Dictionary<int, string>
{
    { 1, "ID must be greater than 0" },
    { 2, "المعرف يجب أن يكون أكبر من 0" }
},

                ["AtLeastOneField"] = new Dictionary<int, string>
{
    { 1, "At least one field must be provided for update" },
    { 2, "يجب توفير حقل واحد على الأقل للتحديث" }
},


                // Infrastructure/Services/LocalizationService.cs

                ["TransactionsGroup"] = new Dictionary<int, string>
{
    { 1, "Transactions Group" },
    { 2, "مجموعة المعاملات" }
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

            return key;
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