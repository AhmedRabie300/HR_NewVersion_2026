// ✨ ملف: VenusHR.API.Endpoints.Users/UserEndpoints.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;
using VenusHR.API.Helpers;
using VenusHR.Application.Common.DTOs.Users;
using VenusHR.Application.Common.Interfaces.Users;

namespace VenusHR.API.Endpoints.Users
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
             app.MapGet("/api/users", GetAllUsers)
                .RequirePermission("users", "View");

            app.MapGet("/api/users/{id:int}", GetUserById)
                .RequirePermission("users", "View");

            app.MapPost("/api/users", CreateUser)
                .RequirePermission("users", "Add");

            app.MapPut("/api/users/{id:int}", UpdateUser)
                .RequirePermission("users", "Edit");

            app.MapDelete("/api/users/{id:int}", DeleteUser)
                .RequirePermission("users", "Delete");

            // 🔹 User Groups
            app.MapGet("/api/users/{userId:int}/groups", GetUserGroups)
                .RequirePermission("users", "View");

            app.MapPost("/api/users/{userId:int}/groups/{groupId:int}", AddUserToGroup)
                .RequirePermission("users", "Edit");

            app.MapDelete("/api/users/{userId:int}/groups/{groupId:int}", RemoveUserFromGroup)
                .RequirePermission("users", "Edit");

             app.MapGet("/api/users/{userId:int}/features", GetUserFeatures)
                .RequirePermission("users", "View");

            app.MapGet("/api/users/{userId:int}/groups/{groupId:int}/features", GetUserFeaturesByGroup)
                .RequirePermission("users", "View");

             app.MapPut("/api/users/{userId:int}/activate", ActivateUser)
                .RequirePermission("users", "Edit");

            app.MapPut("/api/users/{userId:int}/deactivate", DeactivateUser)
                .RequirePermission("users", "Edit");

            app.MapGet("/api/users/{userId:int}/is-admin", CheckIsAdmin)
                .RequirePermission("users", "View");

            app.MapGet("/api/users/{userId:int}/is-active", CheckIsActive)
                .RequirePermission("users", "View");

             app.MapPut("/api/users/{userId:int}/device-token", UpdateDeviceToken)
                .RequirePermission("users", "Edit");
        }

        // ============ ✅ User CRUD ============

        private static async Task<IResult> GetAllUsers(
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.GetAllUsersAsync();

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    message = "Users retrieved successfully",
                    data = result.Data
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        private static async Task<IResult> GetUserById(
            int id,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.GetUserByIdAsync(id);

                if (!result.Success)
                    return Results.NotFound(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    message = "User retrieved successfully",
                    data = result.Data
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        private static async Task<IResult> CreateUser(
            [FromBody] CreateUserDto dto,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.CreateUserAsync(dto);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Created($"/api/users/{result.Data?.Id}", new
                {
                    success = true,
                    message = "User created successfully",
                    data = result.Data
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        private static async Task<IResult> UpdateUser(
            int id,
            [FromBody] UpdateUserDto dto,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.UpdateUserAsync(id, dto);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    message = "User updated successfully",
                    data = result.Data
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        private static async Task<IResult> DeleteUser(
            int id,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.DeleteUserAsync(id);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    message = "User deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // ============ ✅ User Groups ============

        private static async Task<IResult> GetUserGroups(
            int userId,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.GetUserGroupsAsync(userId);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    message = "User groups retrieved successfully",
                    data = result.Data
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        private static async Task<IResult> AddUserToGroup(
           int userId,
    int groupId,
    [FromServices] IUserService userService,
    [FromQuery] bool isPrimary = false)

        {
            try
            {
                var result = await userService.AddUserToGroupAsync(userId, groupId, isPrimary);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    message = "User added to group successfully"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        private static async Task<IResult> RemoveUserFromGroup(
            int userId,
            int groupId,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.RemoveUserFromGroupAsync(userId, groupId);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    message = "User removed from group successfully"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // ============ ✅ User Features ============

        private static async Task<IResult> GetUserFeatures(
            int userId,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.GetUserFeaturesAsync(userId);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    message = "User features retrieved successfully",
                    data = result.Data
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        private static async Task<IResult> GetUserFeaturesByGroup(
            int userId,
            int groupId,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.GetUserFeaturesByGroupAsync(userId, groupId);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    message = "Group features retrieved successfully",
                    data = result.Data
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // ============ ✅ User Status ============

        private static async Task<IResult> ActivateUser(
            int userId,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.UpdateUserStatusAsync(userId, true);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    message = "User activated successfully"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        private static async Task<IResult> DeactivateUser(
            int userId,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.UpdateUserStatusAsync(userId, false);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    message = "User deactivated successfully"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        private static async Task<IResult> CheckIsAdmin(
            int userId,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.IsAdminAsync(userId);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    isAdmin = result.Data
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        private static async Task<IResult> CheckIsActive(
            int userId,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.IsActiveAsync(userId);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    isActive = result.Data
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // ============ ✅ User Device ============

        private static async Task<IResult> UpdateDeviceToken(
            int userId,
            [FromBody] string deviceToken,
            [FromServices] IUserService userService)
        {
            try
            {
                var result = await userService.UpdateDeviceTokenAsync(userId, deviceToken);

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message });

                return Results.Ok(new
                {
                    success = true,
                    message = "Device token updated successfully"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}