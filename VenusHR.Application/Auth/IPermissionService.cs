 using VenusHR.Application.Common.DTOs.Login;

namespace VenusHR.Application.Common.Interfaces.Auth
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(int userId, string featureName, string action);
        Task<List<UserFeatureDto>> GetUserPermissionsAsync(int userId);
        Task<bool> HasAnyPermissionAsync(int userId, List<string> features);
        Task<Dictionary<string, bool>> GetMultiplePermissionsAsync(int userId, Dictionary<string, string> featureActions);
    }
}