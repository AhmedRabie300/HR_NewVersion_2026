using VenusHR.Application.Common.DTOs.Groups;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.DTOs.Users;

namespace VenusHR.Application.Common.Interfaces.Groups;

public interface IGroupService
{
    Task<ApiResponse<GroupDto>> GetGroupByIdAsync(int id);
    Task<ApiResponse<List<GroupDto>>> GetAllGroupsAsync();
    Task<ApiResponse<GroupDto>> CreateGroupAsync(CreateGroupDto dto);
    Task<ApiResponse<GroupDto>> UpdateGroupAsync(int id, UpdateGroupDto dto);
    Task<ApiResponse<bool>> DeleteGroupAsync(int id);
    Task<ApiResponse<List<UserDto>>> GetGroupUsersAsync(int groupId);
    Task<ApiResponse<List<GroupFeatureDto>>> GetGroupFeaturesAsync(int groupId);
}