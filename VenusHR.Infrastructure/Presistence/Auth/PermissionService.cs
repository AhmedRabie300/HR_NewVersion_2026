// ✨ ملف: VenusHR.Infrastructure.Services.Auth/PermissionService.cs
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using VenusHR.Application.Common.DTOs.Login;
using VenusHR.Application.Common.Interfaces.Auth;
using VenusHR.Application.Common.Interfaces.Users;

namespace VenusHR.Infrastructure.Services.Auth
{
    public class PermissionService : IPermissionService
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(
            IUserService userService,
            IMemoryCache cache,
            ILogger<PermissionService> logger)
        {
            _userService = userService;
            _cache = cache;
            _logger = logger;
        }

        public async Task<bool> HasPermissionAsync(int userId, string featureName, string action)
        {
            try
            {
                // 1. نشوف لو Admin
                var isAdminResult = await _userService.IsAdminAsync(userId);
                if (isAdminResult.Success && isAdminResult.Data)
                    return true;

                // 2. نجيب Features اليوزر
                var features = await GetUserPermissionsAsync(userId);

                // 3. نفحص الصلاحية
                return features.Any(f =>
                    f.FeatureName.Equals(featureName, StringComparison.OrdinalIgnoreCase) &&
                    HasAction(f, action));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking permission for user {UserId}", userId);
                return false;
            }
        }

        public async Task<List<UserFeatureDto>> GetUserPermissionsAsync(int userId)
        {
            var cacheKey = $"UserPermissions_{userId}";

            if (!_cache.TryGetValue(cacheKey, out List<UserFeatureDto> features))
            {
                var result = await _userService.GetUserFeaturesAsync(userId);
                features = result.Success ? result.Data ?? new() : new();

                _cache.Set(cacheKey, features, TimeSpan.FromMinutes(10));
            }

            return features;
        }

        private bool HasAction(UserFeatureDto feature, string action)
        {
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

        public async Task<bool> HasAnyPermissionAsync(int userId, List<string> features)
        {
            foreach (var feature in features)
            {
                // TODO: Parse feature string "BloodGroups_View"
                var parts = feature.Split('_');
                if (parts.Length == 2)
                {
                    if (await HasPermissionAsync(userId, parts[0], parts[1]))
                        return true;
                }
            }
            return false;
        }

        public async Task<Dictionary<string, bool>> GetMultiplePermissionsAsync(
            int userId,
            Dictionary<string, string> featureActions)
        {
            var result = new Dictionary<string, bool>();

            foreach (var (feature, action) in featureActions)
            {
                result[$"{feature}_{action}"] = await HasPermissionAsync(userId, feature, action);
            }

            return result;
        }
    }
}