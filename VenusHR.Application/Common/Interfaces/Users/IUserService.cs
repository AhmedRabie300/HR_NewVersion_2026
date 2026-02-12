// ✨ ملف: VenusHR.Application.Common.Interfaces.Users/IUserService.cs
using VenusHR.Application.Common.DTOs.Features;
using VenusHR.Application.Common.DTOs.Login;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.DTOs.Users;

namespace VenusHR.Application.Common.Interfaces.Users
{
    public interface IUserService
    {
         Task<ApiResponse<UserDto>> GetUserByIdAsync(int id);
        Task<ApiResponse<List<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto dto);
        Task<ApiResponse<UserDto>> UpdateUserAsync(int id, UpdateUserDto dto);
        Task<ApiResponse<bool>> DeleteUserAsync(int id);

         Task<ApiResponse<List<UserGroupDto>>> GetUserGroupsAsync(int userId);
        Task<ApiResponse<bool>> AddUserToGroupAsync(int userId, int groupId, bool isPrimary = false);
        Task<ApiResponse<bool>> RemoveUserFromGroupAsync(int userId, int groupId);

         Task<ApiResponse<List<UserFeatureDto>>> GetUserFeaturesAsync(int userId);
        Task<ApiResponse<List<UserFeatureDto>>> GetUserFeaturesByGroupAsync(int userId, int groupId);

         Task<ApiResponse<bool>> IsAdminAsync(int userId);
        Task<ApiResponse<bool>> IsActiveAsync(int userId);
        Task<ApiResponse<bool>> UpdateUserStatusAsync(int userId, bool isActive);

         Task<ApiResponse<bool>> UpdateDeviceTokenAsync(int userId, string deviceToken);
    }
}