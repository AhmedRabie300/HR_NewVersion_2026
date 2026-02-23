 using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenusHR.Application.Common.DTOs.Features;
using VenusHR.Application.Common.DTOs.Login;
using VenusHR.Application.Common.DTOs.Shared;
using VenusHR.Application.Common.DTOs.Users;
using VenusHR.Application.Common.Interfaces.Users;
using VenusHR.Core.Login;
using WorkFlow_EF;

namespace VenusHR.Infrastructure.Presistence.Users
{
    public class UserService : IUserService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDBContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

 
        public async Task<ApiResponse<List<UserGroupDto>>> GetUserGroupsAsync(int userId)
        {
            try
            {
                var groups = await _context.Sys_GroupsUsers
                    .Where(gu => gu.UserId == userId)
                    .Join(_context.Sys_Groups,
                        gu => gu.GroupId,
                        g => g.Id,
                        (gu, g) => new UserGroupDto
                        {
                            Id = g.Id,
                            Code = g.Code,
                            ArbName = g.ArbName,
                            EngName = g.EngName
                        })
                    .ToListAsync();

                return ApiResponse<List<UserGroupDto>>.Succeeded(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting groups for user {UserId}", userId);
                return ApiResponse<List<UserGroupDto>>.Failed("Failed to get user groups");
            }
        }

        public async Task<ApiResponse<bool>> AddUserToGroupAsync(int userId, int groupId, bool isPrimary = false)
        {
            try
            {
                var existing = await _context.Sys_GroupsUsers
                    .FirstOrDefaultAsync(gu => gu.UserId == userId && gu.GroupId == groupId);

                if (existing != null)
                {
                    return ApiResponse<bool>.Failed("User already in this group");
                }

                var userGroup = new Sys_GroupsUsers
                {
                    UserId = userId,
                    GroupId = groupId,
                    IsPrimary = isPrimary,
                    JoinedDate = DateTime.Now,
                    RegDate = DateTime.Now
                };

                await _context.Sys_GroupsUsers.AddAsync(userGroup);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Succeeded(true, "User added to group successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user {UserId} to group {GroupId}", userId, groupId);
                return ApiResponse<bool>.Failed("Failed to add user to group");
            }
        }

        public async Task<ApiResponse<bool>> RemoveUserFromGroupAsync(int userId, int groupId)
        {
            try
            {
                var userGroup = await _context.Sys_GroupsUsers
                    .FirstOrDefaultAsync(gu => gu.UserId == userId && gu.GroupId == groupId);

                if (userGroup == null)
                {
                    return ApiResponse<bool>.Failed("User not found in this group");
                }

                _context.Sys_GroupsUsers.Remove(userGroup);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Succeeded(true, "User removed from group successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing user {UserId} from group {GroupId}", userId, groupId);
                return ApiResponse<bool>.Failed("Failed to remove user from group");
            }
        }
        public async Task<ApiResponse<List<UserFeatureDto>>> GetUserFeaturesAsync(int userId)
        {
            try
            {
                 var userGroups = await GetUserGroupsAsync(userId);
                if (!userGroups.Success || userGroups.Data == null || !userGroups.Data.Any())
                {
                    return ApiResponse<List<UserFeatureDto>>.Succeeded(new List<UserFeatureDto>());
                }

                var groupIds = userGroups.Data.Select(g => g.Id).ToList();

                 var formsPermissions = await _context.sys_FormsPermissions
                    .Where(fp => groupIds.Contains(fp.GroupID.Value) && fp.CancelDate == null)
                    .Include(fp => fp.Form)
                    .ToListAsync();

                 var userPermissions = await _context.sys_FormsPermissions
                    .Where(fp => fp.UserID == userId && fp.CancelDate == null)
                    .Include(fp => fp.Form)
                    .ToListAsync();

                 var permissionsDict = new Dictionary<int, UserFeatureDto>();

                 foreach (var fp in formsPermissions)
                {
                    if (fp.Form == null || fp.Form.CancelDate != null)
                        continue;

                    var formId = fp.Form.ID;

                    if (!permissionsDict.ContainsKey(formId))
                    {
                        permissionsDict[formId] = new UserFeatureDto
                        {
                            FeatureId = formId,
                            FeatureName = fp.Form.EngName ?? fp.Form.ArbName ?? "Unknown",
                            ArabicName = fp.Form.ArbName,
                            EnglishName = fp.Form.EngName,
                            ModuleId = fp.Form.ModuleID,
                            Hidden = false
                        };
                    }

                    var form = permissionsDict[formId];

                     form.View |= fp.AllowView ?? false;
                    form.Add |= fp.AllowAdd ?? false;
                    form.Edit |= fp.AllowEdit ?? false;
                    form.Delete |= fp.AllowDelete ?? false;
                    form.Print |= fp.AllowPrint ?? false;
                 }

                 foreach (var fp in userPermissions)
                {
                    if (fp.Form == null || fp.Form.CancelDate != null)
                        continue;

                    var formId = fp.Form.ID;

                    if (!permissionsDict.ContainsKey(formId))
                    {
                        permissionsDict[formId] = new UserFeatureDto
                        {
                            FeatureId = formId,
                            FeatureName = fp.Form.EngName ?? fp.Form.ArbName ?? "Unknown",
                            ArabicName = fp.Form.ArbName,
                            EnglishName = fp.Form.EngName,
                            ModuleId = fp.Form.ModuleID,
                            Hidden = false
                        };
                    }

                    var form = permissionsDict[formId];

                     if (fp.AllowView != null) form.View = fp.AllowView.Value;
                    if (fp.AllowAdd != null) form.Add = fp.AllowAdd.Value;
                    if (fp.AllowEdit != null) form.Edit = fp.AllowEdit.Value;
                    if (fp.AllowDelete != null) form.Delete = fp.AllowDelete.Value;
                    if (fp.AllowPrint != null) form.Print = fp.AllowPrint.Value;
                }

                 var result = permissionsDict.Values
                    .Where(f => f.View || f.Add || f.Edit || f.Delete || f.Print)
                    .ToList();

                return ApiResponse<List<UserFeatureDto>>.Succeeded(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting features for user {UserId}", userId);
                return ApiResponse<List<UserFeatureDto>>.Failed("Failed to get user features");
            }
        }


        //public async Task<ApiResponse<List<UserFeatureDto>>> GetUserFeaturesAsync(int userId)
        //{
        //    try
        //    {
        //        var userGroups = await GetUserGroupsAsync(userId);
        //        if (!userGroups.Success || userGroups.Data == null || !userGroups.Data.Any())
        //        {
        //            return ApiResponse<List<UserFeatureDto>>.Succeeded(new List<UserFeatureDto>());
        //        }

        //        var groupIds = userGroups.Data.Select(g => g.Id).ToList();

        //        var groupFeatures = await _context.Sys_GroupFeatures
        //            .Where(gf => groupIds.Contains(gf.GroupId))
        //            .Include(gf => gf.Feature)
        //            .ToListAsync();

        //        var featuresDict = new Dictionary<int, UserFeatureDto>();

        //        foreach (var gf in groupFeatures)
        //        {
        //            if (gf.Feature == null || !(gf.Feature.IsActive ?? true))
        //                continue;

        //            var featureId = gf.Feature.ID;

        //            if (!featuresDict.ContainsKey(featureId))
        //            {
        //                featuresDict[featureId] = new UserFeatureDto
        //                {
        //                    FeatureId = featureId,
        //                    FeatureName = gf.Feature.EnglishName ?? gf.Feature.ArabicName ?? "Unknown",
        //                    ArabicName = gf.Feature.ArabicName,
        //                    EnglishName = gf.Feature.EnglishName,
        //                    ModuleId = gf.Feature.ModuleID,
        //                    Hidden = false
        //                };
        //            }

        //            var feature = featuresDict[featureId];

        //            feature.View |= gf.View ?? false;
        //            feature.Add |= gf.Add ?? false;
        //            feature.Edit |= gf.Edit ?? false;
        //            feature.Delete |= gf.Delete ?? false;
        //            feature.Export |= gf.Export ?? false;
        //            feature.Print |= gf.Print ?? false;
        //        }

        //        return ApiResponse<List<UserFeatureDto>>.Succeeded(featuresDict.Values.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error getting features for user {UserId}", userId);
        //        return ApiResponse<List<UserFeatureDto>>.Failed("Failed to get user features");
        //    }
        //}

        public async Task<ApiResponse<List<UserFeatureDto>>> GetUserFeaturesByGroupAsync(int userId, int groupId)
        {
            try
            {
                var groupFeatures = await _context.Sys_GroupFeatures
                    .Where(gf => gf.GroupId == groupId)
                    .Include(gf => gf.Feature)
                    .ToListAsync();

                var features = groupFeatures
                    .Where(gf => gf.Feature != null && (gf.Feature.IsActive ?? true))
                    .Select(gf => new UserFeatureDto
                    {
                        FeatureId = gf.Feature.ID,
                        FeatureName = gf.Feature.EnglishName ?? gf.Feature.ArabicName ?? "Unknown",
                        ArabicName = gf.Feature.ArabicName,
                        EnglishName = gf.Feature.EnglishName,
                        ModuleId = gf.Feature.ModuleID,
                        View = gf.View ?? false,
                        Add = gf.Add ?? false,
                        Edit = gf.Edit ?? false,
                        Delete = gf.Delete ?? false,
                        Export = gf.Export ?? false,
                        Print = gf.Print ?? false,
                        Hidden = false
                    })
                    .ToList();

                return ApiResponse<List<UserFeatureDto>>.Succeeded(features);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting features for group {GroupId}", groupId);
                return ApiResponse<List<UserFeatureDto>>.Failed("Failed to get group features");
            }
        }

 
        public async Task<ApiResponse<bool>> IsAdminAsync(int userId)
        {
            try
            {
                var isAdmin = await _context.Sys_Users
                    .Where(u => u.Id == userId)
                    .Select(u => u.IsAdmin ?? false)
                    .FirstOrDefaultAsync();

                return ApiResponse<bool>.Succeeded(isAdmin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking admin status for user {UserId}", userId);
                return ApiResponse<bool>.Failed("Failed to check admin status");
            }
        }

        public async Task<ApiResponse<bool>> IsActiveAsync(int userId)
        {
            try
            {
                var isActive = await _context.Sys_Users
                    .Where(u => u.Id == userId)
                    .Select(u => u.IsActive ?? true)
                    .FirstOrDefaultAsync();

                return ApiResponse<bool>.Succeeded(isActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking active status for user {UserId}", userId);
                return ApiResponse<bool>.Failed("Failed to check user status");
            }
        }

        public async Task<ApiResponse<bool>> UpdateUserStatusAsync(int userId, bool isActive)
        {
            try
            {
                var user = await _context.Sys_Users.FindAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.Failed("User not found");
                }

                user.IsActive = isActive;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Succeeded(true, "User status updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user status for user {UserId}", userId);
                return ApiResponse<bool>.Failed("Failed to update user status");
            }
        }

 
        public async Task<ApiResponse<bool>> UpdateDeviceTokenAsync(int userId, string deviceToken)
        {
            try
            {
                var user = await _context.Sys_Users.FindAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.Failed("User not found");
                }

                user.DeviceToken = deviceToken;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Succeeded(true, "Device token updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating device token for user {UserId}", userId);
                return ApiResponse<bool>.Failed("Failed to update device token");
            }
        }

 
         public async Task<ApiResponse<UserDto>> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _context.Sys_Users
                    .Where(u => u.Id == id)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Code = u.Code,
                        EngName = u.EngName,
                        ArbName = u.ArbName,
                        IsAdmin = u.IsAdmin ?? false,
                        IsActive = u.IsActive ?? true,
                        DeviceToken = u.DeviceToken
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return ApiResponse<UserDto>.Failed("User not found");
                }

                 var groupsResult = await GetUserGroupsAsync(id);
                if (groupsResult.Success && groupsResult.Data != null)
                {
                    user.Groups = groupsResult.Data.Cast<UserGroupInfoDto>().ToList();
                 }

                return ApiResponse<UserDto>.Succeeded(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id {UserId}", id);
                return ApiResponse<UserDto>.Failed("Failed to get user");
            }
        }
        public async Task<ApiResponse<List<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Sys_Users
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Code = u.Code,
                        EngName = u.EngName,
                        ArbName = u.ArbName,
                        IsAdmin = u.IsAdmin ?? false,
                        IsActive = u.IsActive ?? true,
                        DeviceToken = u.DeviceToken
                    })
                    .ToListAsync();

                return ApiResponse<List<UserDto>>.Succeeded(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return ApiResponse<List<UserDto>>.Failed("Failed to get users");
            }
        }

        public async Task<ApiResponse<UserDto>> CreateUserAsync(UserDto dto)
        {
            try
            {
                 var existingUser = await _context.Sys_Users
                    .FirstOrDefaultAsync(u => u.Code == dto.Code);

                if (existingUser != null)
                {
                    return ApiResponse<UserDto>.Failed("User with this code already exists");
                }

                var user = new Sys_Users
                {
                    Code = dto.Code,
                    EngName = dto.EngName,
                    ArbName = dto.ArbName,
                     IsAdmin = dto.IsAdmin,
                     DeviceToken = dto.DeviceToken,
                    RegDate = DateTime.Now
                };

                await _context.Sys_Users.AddAsync(user);
                await _context.SaveChangesAsync();

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Code = user.Code,
                    EngName = user.EngName,
                    ArbName = user.ArbName,
                    IsAdmin = user.IsAdmin ?? false,
                    IsActive = user.IsActive ?? true,
                    DeviceToken = user.DeviceToken
                };

                return ApiResponse<UserDto>.Succeeded(userDto, "User created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return ApiResponse<UserDto>.Failed("Failed to create user");
            }
        }

        public async Task<ApiResponse<UserDto>> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            try
            {
                var user = await _context.Sys_Users.FindAsync(id);
                if (user == null)
                {
                    return ApiResponse<UserDto>.Failed("User not found");
                }

                user.EngName = dto.EngName ?? user.EngName;
                user.ArbName = dto.ArbName ?? user.ArbName;
                user.IsAdmin = dto.IsAdmin ?? user.IsAdmin;
               // user.IsActive = dto.IsActive ?? user.IsActive;

             

                await _context.SaveChangesAsync();

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Code = user.Code,
                    EngName = user.EngName,
                    ArbName = user.ArbName,
                    IsAdmin = user.IsAdmin ?? false,
                    IsActive = user.IsActive ?? true,
                    DeviceToken = user.DeviceToken
                };

                return ApiResponse<UserDto>.Succeeded(userDto, "User updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                return ApiResponse<UserDto>.Failed("Failed to update user");
            }
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Sys_Users.FindAsync(id);
                if (user == null)
                {
                    return ApiResponse<bool>.Failed("User not found");
                }

                _context.Sys_Users.Remove(user);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Succeeded(true, "User deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", id);
                return ApiResponse<bool>.Failed("Failed to delete user");
            }
        }
    }
}