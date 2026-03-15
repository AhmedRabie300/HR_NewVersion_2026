using Application.Common.Abstractions;  
using Application.UARbac.Abstractions;
using Application.UARbac.FormPermission.Dtos;

namespace API.Helpers
{
    public class PermissionRequirement
    {
        public string FeatureName { get; }
        public string Action { get; }
        public PermissionRequirement(string featureName, string action)
        {
            FeatureName = featureName;
            Action = action;
        }
    }

    public static class RouteHandlerBuilderExtensions
    {
        public static RouteHandlerBuilder RequirePermission(
            this RouteHandlerBuilder builder,
            string featureName,
            string action)
        {
            return builder.WithMetadata(new PermissionRequirement(featureName, action));
        }
    }

    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PermissionMiddleware> _logger;
        private readonly ILocalizationService _localizer;

        public PermissionMiddleware(
            RequestDelegate next,
            ILogger<PermissionMiddleware> logger,
            ILocalizationService localizer)
        {
            _next = next;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task InvokeAsync(HttpContext context, IFormPermissionRepository permissionService)
        {
            var endpoint = context.GetEndpoint();
            var permission = endpoint?.Metadata.GetMetadata<PermissionRequirement>();

             int lang = GetLanguage(context);

            if (permission != null)
            {
                var userId = context.User.FindFirst("userId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = _localizer.GetMessage("Unauthorized", lang)
                    });
                    return;
                }

                var isAdmin = context.User.FindFirst("isAdmin")?.Value == "True";
                if (isAdmin)
                {
                    _logger.LogInformation($"Admin access granted for user {userId}");
                    await _next(context);
                    return;
                }

                _logger.LogInformation($"Checking permissions for user {userId}, Feature: {permission.FeatureName}, Action: {permission.Action}");

                var userPermissions = await permissionService.GetUserEffectivePermissionsAsync(int.Parse(userId));

                var permissions = userPermissions.Select(p => new UserFormPermissionDto(
                    p.FormId,
                    p.Form?.Code ?? "",
                    p.Form?.EngName ?? p.Form?.ArbName ?? "",
                    p.AllowView ?? false,
                    p.AllowAdd ?? false,
                    p.AllowEdit ?? false,
                    p.AllowDelete ?? false,
                    p.AllowPrint ?? false,
                    p.UserId.HasValue ? "User" : (p.GroupId.HasValue ? "Group" : "Unknown")
                )).ToList();

                if (!permissions.Any())
                {
                    _logger.LogWarning($"No permissions found for user {userId}");
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = _localizer.GetMessage("AccessDenied", lang)
                    });
                    return;
                }

                var hasPermission = CheckPermission(permissions, permission.FeatureName, permission.Action);

                if (!hasPermission)
                {
                    _logger.LogWarning($"Access denied for user {userId} - Feature: {permission.FeatureName}, Action: {permission.Action}");

                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = string.Format(
                            _localizer.GetMessage("PermissionDenied", lang),
                            GetActionName(permission.Action, lang)
                        ),
                        requiredFeature = permission.FeatureName,
                        requiredAction = permission.Action
                    });
                    return;
                }

                _logger.LogInformation($"Access granted for user {userId} - Feature: {permission.FeatureName}, Action: {permission.Action}");
            }

            await _next(context);
        }

        private int GetLanguage(HttpContext context)
        {
             if (context.Request.Query.TryGetValue("lang", out var langValue))
            {
                if (int.TryParse(langValue, out int lang))
                    return lang == 2 ? 2 : 1;
            }

             if (context.Request.Headers.TryGetValue("Accept-Language", out var headerLang))
            {
                if (headerLang.ToString().StartsWith("ar"))
                    return 2;
            }

            return 1; 
        }

        private bool CheckPermission(List<UserFormPermissionDto> permissions, string featureName, string action)
        {
            _logger.LogInformation($" Searching for feature: '{featureName}'");

            var permission = permissions.FirstOrDefault(p =>
                p.FormCode?.Equals(featureName, StringComparison.OrdinalIgnoreCase) == true ||
                p.FormName?.Contains(featureName, StringComparison.OrdinalIgnoreCase) == true);

            if (permission == null)
            {
                _logger.LogWarning($" Feature '{featureName}' not found");
                return false;
            }

            _logger.LogInformation($" Found feature: '{permission.FormCode}' (ID: {permission.FormId})");

            var result = action.ToLower() switch
            {
                "view" => permission.AllowView,
                "add" => permission.AllowAdd,
                "edit" => permission.AllowEdit,
                "delete" => permission.AllowDelete,
                "print" => permission.AllowPrint,
                _ => false
            };

            _logger.LogInformation($" Permission check - Feature: {permission.FormCode}, Action: {action}, Result: {result}");
            return result;
        }

        private string GetActionName(string action, int lang)
        {
            var actionNames = new Dictionary<string, Dictionary<int, string>>
            {
                ["view"] = new Dictionary<int, string> { { 1, "View" }, { 2, "عرض" } },
                ["add"] = new Dictionary<int, string> { { 1, "Add" }, { 2, "إضافة" } },
                ["edit"] = new Dictionary<int, string> { { 1, "Edit" }, { 2, "تعديل" } },
                ["delete"] = new Dictionary<int, string> { { 1, "Delete" }, { 2, "حذف" } },
                ["print"] = new Dictionary<int, string> { { 1, "Print" }, { 2, "طباعة" } }
            };

            var key = action.ToLower();
            if (actionNames.ContainsKey(key) && actionNames[key].ContainsKey(lang))
                return actionNames[key][lang];

            return action;
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UsePermissionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<PermissionMiddleware>();
        }
    }
}