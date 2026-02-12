//// ✨ ملف: VenusHR.Infrastructure.Presistence.Users/UserService.cs
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System.Linq;
//using VenusHR.Application.Common.DTOs.Login;
//using VenusHR.Application.Common.DTOs.Shared;
//using VenusHR.Application.Common.DTOs.Users;
//using VenusHR.Application.Common.Interfaces.Users;
//using VenusHR.Application.Common.DTOs.Features;
//using VenusHR.Core.Login;
//using WorkFlow_EF;

//namespace VenusHR.Infrastructure.Presistence.Users
//{
//    public class UserService : IUserService
//    {
//        private readonly ApplicationDBContext _context;
//        private readonly ILogger<UserService> _logger;

//        public UserService(ApplicationDBContext context, ILogger<UserService> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }

//        // ✅ تعديل GetUserGroupsAsync
//        public async Task<ApiResponse<List<UserGroupDto>>> GetUserGroupsAsync(int userId)
//        {
//            try
//            {
//                var groups = await _context.Sys_GroupsUsers
//                    .Where(gu => gu.UserId == userId)
//                    .Select(gu => new UserGroupDto
//                    {
//                        Id = gu.GroupId,           // لو Group مش موجودة، جرب gu.GroupId
//                         ArbName = gu.,
//                        EngName = gu.Group.EngName,
//                    })
//                    .ToListAsync();

//                return ApiResponse<List<UserGroupDto>>.Success(groups);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting groups for user {UserId}", userId);
//                return ApiResponse<List<UserGroupDto>>.Fail("Failed to get user groups");
//            }
//        }
//        // ✅ Get User Features
//        public async Task<ApiResponse<List<UserFeatureDto>>> GetUserFeaturesAsync(int userId)
//        {
//            try
//            {
//                // 1. نجيب Groups اليوزر
//                var userGroups = await GetUserGroupsAsync(userId);
//                if (!userGroups.Success || userGroups.Data == null || !userGroups.Data.Any())
//                {
//                    return ApiResponse<List<UserFeatureDto>>.Success(new List<UserFeatureDto>());
//                }

//                var groupIds = userGroups.Data.Select(g => g.Id).ToList();

//                // 2. نجيب Features بتاعة الـ Groups
//                var groupFeatures = await _context.Sys_GroupFeatures
//                    .Where(gf => groupIds.Contains(gf.GroupId))
//                    .Include(gf => gf.Feature)
//                    .ToListAsync();

//                // 3. دمج الـ Features
//                var featuresDict = new Dictionary<int, UserFeatureDto>();

//                foreach (var gf in groupFeatures)
//                {
//                    if (gf.Feature == null || !(gf.Feature.IsActive ?? true))
//                        continue;

//                    var featureId = gf.Feature.ID;

//                    if (!featuresDict.ContainsKey(featureId))
//                    {
//                        featuresDict[featureId] = new UserFeatureDto
//                        {
//                            FeatureId = featureId,
//                            FeatureName = gf.Feature.EnglishName ?? gf.Feature.ArabicName ?? "Unknown",
//                            ArabicName = gf.Feature.ArabicName,
//                            EnglishName = gf.Feature.EnglishName,
//                            ModuleId = gf.Feature.ModuleID,
//                            Hidden = false
//                        };
//                    }

//                    var feature = featuresDict[featureId];

//                    // OR operations - لو أي Group عنده الصلاحية
//                    feature.View |= gf.View ?? false;
//                    feature.Add |= gf.Add ?? false;
//                    feature.Edit |= gf.Edit ?? false;
//                    feature.Delete |= gf.Delete ?? false;
//                    feature.Export |= gf.Export ?? false;
//                    feature.Print |= gf.Print ?? false;
//                }

//                return ApiResponse<List<UserFeatureDto>>.Success(featuresDict.Values.ToList());
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting features for user {UserId}", userId);
//                return ApiResponse<List<UserFeatureDto>>.Fail("Failed to get user features");
//            }
//        }

//        // ✅ Check if user is admin
//        public async Task<ApiResponse<bool>> IsAdminAsync(int userId)
//        {
//            try
//            {
//                var user = await _context.Sys_Users
//                    .Where(u => u.Id == userId)
//                    .Select(u => u.IsAdmin ?? false)
//                    .FirstOrDefaultAsync();

//                return ApiResponse<bool>.Success(user);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error checking admin status for user {UserId}", userId);
//                return ApiResponse<bool>.Fail("Failed to check admin status");
//            }
//        }

//        // ✅ Update device token
//        public async Task<ApiResponse<bool>> UpdateDeviceTokenAsync(int userId, string deviceToken)
//        {
//            try
//            {
//                var user = await _context.Sys_Users.FindAsync(userId);
//                if (user == null)
//                {
//                    return ApiResponse<bool>.Fail("User not found");
//                }

//                user.DeviceToken = deviceToken;
//                await _context.SaveChangesAsync();

//                return ApiResponse<bool>.Success(true);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating device token for user {UserId}", userId);
//                return ApiResponse<bool>.Fail("Failed to update device token");
//            }
//        }

//        // ... باقي الـ CRUD operations ...
//    }
//}