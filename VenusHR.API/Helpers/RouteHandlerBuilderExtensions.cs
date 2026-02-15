using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenusHR.Application.Common.DTOs.Features;
using VenusHR.Application.Common.DTOs.Login;
using VenusHR.Application.Common.Interfaces.Users;

namespace VenusHR.API.Helpers
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

        public PermissionMiddleware(RequestDelegate next, ILogger<PermissionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

         public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            var endpoint = context.GetEndpoint();
            var permission = endpoint?.Metadata.GetMetadata<PermissionRequirement>();

            if (permission != null)
            {
                var userId = context.Items["UserId"] as string;
                if (string.IsNullOrEmpty(userId))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Unauthorized: User not authenticated"
                    });
                    return;
                }

                var isAdmin = context.Items["IsAdmin"] as string == "True";
                if (isAdmin)
                {
                    _logger.LogInformation($"Admin access granted for user {userId}");
                    await _next(context);
                    return;
                }

                 _logger.LogInformation($"Checking permissions for user {userId}, Feature: {permission.FeatureName}, Action: {permission.Action}");

                var featuresResult = await userService.GetUserFeaturesAsync(int.Parse(userId));

                if (!featuresResult.Success || featuresResult.Data == null)
                {
                    _logger.LogWarning($"Failed to get features for user {userId}: {featuresResult.Message}");
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Access denied. Unable to retrieve permissions."
                    });
                    return;
                }

                _logger.LogInformation($"User {userId} has {featuresResult.Data.Count} features");

                var hasPermission = CheckPermission(featuresResult.Data, permission.FeatureName, permission.Action);

                if (!hasPermission)
                {
                    _logger.LogWarning($"Access denied for user {userId} - Feature: {permission.FeatureName}, Action: {permission.Action}");

                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = $"ليس لديك صلاحية '{GetActionName(permission.Action)}' لـ '{GetFeatureName(permission.FeatureName, featuresResult.Data)}'",
                        requiredFeature = permission.FeatureName,
                        requiredAction = permission.Action
                    });
                    return;
                }

                _logger.LogInformation($"Access granted for user {userId} - Feature: {permission.FeatureName}, Action: {permission.Action}");
            }

            await _next(context);
        }

        private bool CheckPermission(List<UserFeatureDto> features, string featureName, string action)
        {
            var feature = features.FirstOrDefault(f =>
                f.EnglishName?.Equals(featureName, StringComparison.OrdinalIgnoreCase) == true ||
                f.ArabicName?.Equals(featureName, StringComparison.OrdinalIgnoreCase) == true);

            if (feature == null)
            {
                _logger.LogWarning($"Feature '{featureName}' not found for user");
                return false;
            }

            var result = action.ToLower() switch
            {
                "view" => feature.View,
                "add" => feature.Add,
                "edit" => feature.Edit,
                "delete" => feature.Delete,
                "export" => feature.Export,
                "print" => feature.Print,
                _ => false
            };

            _logger.LogDebug($"Permission check - Feature: {featureName}, Action: {action}, Result: {result}");
            return result;
        }

        private string GetActionName(string action)
        {
            return action.ToLower() switch
            {
                "view" => "عرض",
                "add" => "إضافة",
                "edit" => "تعديل",
                "delete" => "حذف",
                "export" => "تصدير",
                "print" => "طباعة",
                _ => action
            };
        }

        private string GetFeatureName(string featureName, List<UserFeatureDto> features)
        {
            var feature = features.FirstOrDefault(f =>
                f.EnglishName?.Equals(featureName, StringComparison.OrdinalIgnoreCase) == true);
            return feature?.ArabicName ?? featureName;
        }
    }

    public static class PermissionMiddlewareExtensions
    {
        public static IApplicationBuilder UsePermissionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<PermissionMiddleware>();
        }
    }
}