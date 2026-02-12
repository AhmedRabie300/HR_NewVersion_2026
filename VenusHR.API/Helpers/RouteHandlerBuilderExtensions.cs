 using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VenusHR.Application.Common.DTOs.Features;
using VenusHR.Application.Common.DTOs.Login;

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

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var permission = endpoint?.Metadata.GetMetadata<PermissionRequirement>();

            if (permission != null)
            {
                // 1. التحقق من User ID
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

                // 3. نجيب Features من الـ Context (اللي جاية من Login)
                var featuresJson = context.Items["UserFeatures"] as string;
                if (string.IsNullOrEmpty(featuresJson))
                {
                    _logger.LogWarning($"No features found in context for user {userId}");
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Access denied. No permissions found."
                    });
                    return;
                }

                 var features = JsonSerializer.Deserialize<List<UserFeatureDto>>(featuresJson);
                if (features == null || !features.Any())
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Access denied. No permissions found."
                    });
                    return;
                }

                 var hasPermission = CheckPermission(features, permission.FeatureName, permission.Action);

                if (!hasPermission)
                {
                    _logger.LogWarning($"Access denied for user {userId} - Feature: {permission.FeatureName}, Action: {permission.Action}");

                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = $"ليس لديك صلاحية '{GetActionName(permission.Action)}' لـ '{GetFeatureName(permission.FeatureName, features)}'",
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
                f.ArabicName?.Equals(featureName, StringComparison.OrdinalIgnoreCase) == true ||
                f.FeatureName?.Equals(featureName, StringComparison.OrdinalIgnoreCase) == true);

            if (feature == null)
                return false;

            return action.ToLower() switch
            {
                "view" => feature.View,
                "add" => feature.Add,
                "edit" => feature.Edit,
                "delete" => feature.Delete,
                "export" => feature.Export,
                "print" => feature.Print,
                _ => false
            };
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
                f.EnglishName?.Equals(featureName, StringComparison.OrdinalIgnoreCase) == true ||
                f.FeatureName?.Equals(featureName, StringComparison.OrdinalIgnoreCase) == true);

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