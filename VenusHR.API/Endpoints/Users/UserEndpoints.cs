using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;
using VenusHR.API.Helpers;
using VenusHR.Application.Common.DTOs.Users;
using VenusHR.Application.Common.Interfaces.Users;
using MediatR;

namespace VenusHR.API.Endpoints.Users
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
             routes.MapGet("/api/GetAllUsers", GetAllUsers)
                .RequirePermission("users", "View");

            routes.MapGet("/api/GetUserById/{id:int}", GetUserById)
                .RequirePermission("users", "View");

            routes.MapPost("/api/CreateUser", CreateUser)
                .RequirePermission("users", "Add");

            routes.MapPut("/api/UpdateUser/{id:int}", UpdateUser)
                .RequirePermission("users", "Edit");

            routes.MapDelete("/api/DeleteUser/{id:int}", DeleteUser)
                .RequirePermission("users", "Delete");

             routes.MapGet("/api/GetUserGroups/{userId:int}/groups", GetUserGroups)
                .RequirePermission("users", "View");

            routes.MapPost("/api/AddUserToGroup/{userId:int}/groups/{groupId:int}", AddUserToGroup)
                .RequirePermission("users", "Edit");

            routes.MapDelete("/api/RemoveUserFromGroup/{userId:int}/groups/{groupId:int}", RemoveUserFromGroup)
                .RequirePermission("users", "Edit");

            // =============== User Features Routes ===============
            routes.MapGet("/api/users/{userId:int}/features", GetUserFeatures)
                .RequirePermission("users", "View");

            routes.MapGet("/api/users/{userId:int}/groups/{groupId:int}/features", GetUserFeaturesByGroup)
                .RequirePermission("users", "View");

            // =============== User Status Routes ===============
            routes.MapPut("/api/users/{userId:int}/activate", ActivateUser)
                .RequirePermission("users", "Edit");

            routes.MapPut("/api/users/{userId:int}/deactivate", DeactivateUser)
                .RequirePermission("users", "Edit");

            routes.MapGet("/api/users/{userId:int}/is-admin", CheckIsAdmin)
                .RequirePermission("users", "View");

            routes.MapGet("/api/users/{userId:int}/is-active", CheckIsActive)
                .RequirePermission("users", "View");

            // =============== Device Token Routes ===============
            routes.MapPut("/api/users/{userId:int}/device-token", UpdateDeviceToken)
                .RequirePermission("users", "Edit");
        }

        // =============== QUERIES ===============

        private static async Task<IResult> GetAllUsers([FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserQueries.GetAllUsersQuery());

                // ApiResponse: Success = true يعني نجاح
                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Ok(new
                {
                    success = true,
                    message = result.Message ?? "Users retrieved successfully",
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
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserQueries.GetUserByIdQuery(id));

                if (!result.Success)
                    return Results.NotFound(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Ok(new
                {
                    success = true,
                    message = result.Message ?? "User retrieved successfully",
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

        private static async Task<IResult> GetUserGroups(
            int userId,
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserQueries.GetUserGroupsQuery(userId));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Ok(new
                {
                    success = true,
                    message = result.Message ?? "User groups retrieved successfully",
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

        private static async Task<IResult> GetUserFeatures(
            int userId,
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserQueries.GetUserFeaturesQuery(userId));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Ok(new
                {
                    success = true,
                    message = result.Message ?? "User features retrieved successfully",
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
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserQueries.GetUserFeaturesByGroupQuery(userId, groupId));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Ok(new
                {
                    success = true,
                    message = result.Message ?? "Group features retrieved successfully",
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

        private static async Task<IResult> CheckIsAdmin(
            int userId,
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserQueries.CheckIsAdminQuery(userId));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

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
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserQueries.CheckIsActiveQuery(userId));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

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

        // =============== COMMANDS ===============

        private static async Task<IResult> CreateUser(
            [FromBody] UserDto dto,
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserCommands.CreateUserCommand(dto));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Created($"/api/users/{result.Data?.Id}", new
                {
                    success = true,
                    message = result.Message ?? "User created successfully",
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
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserCommands.UpdateUserCommand(id, dto));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Ok(new
                {
                    success = true,
                    message = result.Message ?? "User updated successfully",
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
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserCommands.DeleteUserCommand(id));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Ok(new
                {
                    success = true,
                    message = result.Message ?? "User deleted successfully"
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
            [FromServices] IMediator mediator,
            [FromQuery] bool isPrimary = false)
        {
            try
            {
                var result = await mediator.Send(new UserCommands.AddUserToGroupCommand(userId, groupId, isPrimary));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Ok(new
                {
                    success = true,
                    message = result.Message ?? "User added to group successfully"
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
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserCommands.RemoveUserFromGroupCommand(userId, groupId));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Ok(new
                {
                    success = true,
                    message = result.Message ?? "User removed from group successfully"
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

        private static async Task<IResult> ActivateUser(
            int userId,
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserCommands.UpdateUserStatusCommand(userId, true));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Ok(new
                {
                    success = true,
                    message = result.Message ?? "User activated successfully"
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
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserCommands.UpdateUserStatusCommand(userId, false));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Ok(new
                {
                    success = true,
                    message = result.Message ?? "User deactivated successfully"
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

        private static async Task<IResult> UpdateDeviceToken(
            int userId,
            [FromBody] string deviceToken,
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new UserCommands.UpdateDeviceTokenCommand(userId, deviceToken));

                if (!result.Success)
                    return Results.BadRequest(new { error = result.Message, errorCode = result.ErrorCode });

                return Results.Ok(new
                {
                    success = true,
                    message = result.Message ?? "Device token updated successfully"
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