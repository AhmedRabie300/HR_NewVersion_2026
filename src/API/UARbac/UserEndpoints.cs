// API/UARbac/UserEndpoints.cs
using Application.UARbac.Users.Commands;
using Application.UARbac.Users.Dtos;
using Application.UARbac.Users.Queries;
using MediatR;
using API.Helpers;

namespace API.UARbac;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var users = routes.MapGroup("/users").WithTags("Users");

        users.MapGet("/",
            async (IMediator mediator, CancellationToken ct) =>
            {
                try
                {
                    Console.WriteLine("Getting all users...");
                    var result = await mediator.Send(new ListUsers.Query(), ct);
                    Console.WriteLine($"Found {result?.Count ?? 0} users");
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR in GetAllUsers: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    return Results.Problem(
                        title: "Server error",
                        detail: ex.Message,
                        statusCode: 500
                    );
                }
            })
          // .RequirePermission("Users", "View")
            .WithName("GetAllUsers");

        users.MapGet("/{id:int}",
            async (IMediator mediator, int id, CancellationToken ct) =>
            {
                try
                {
                    Console.WriteLine($"Getting user with id: {id}");
                    var result = await mediator.Send(new GetUserById.Query(id), ct);
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR in GetUserById: {ex.Message}");
                    return Results.Problem(
                        title: "Server error",
                        detail: ex.Message,
                        statusCode: 500
                    );
                }
            })
           // .RequirePermission("Users", "View")
            .WithName("GetUserById");

        // POST - إنشاء مستخدم جديد
        users.MapPost("/",
            async (IMediator mediator, CreateUserDto dto, CancellationToken ct) =>
            {
                try
                {
                    Console.WriteLine("Creating new user...");
                    var id = await mediator.Send(new Create.Command(dto), ct);
                    return Results.Created($"/users/{id}", new { id });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR in CreateUser: {ex.Message}");
                    return Results.Problem(
                        title: "Server error",
                        detail: ex.Message,
                        statusCode: 500
                    );
                }
            })
           // .RequirePermission("Users", "Add")
            .WithName("CreateUser");

        users.MapPut("/{id:int}",
            async (IMediator mediator, int id, UpdateUserDto dto, CancellationToken ct) =>
            {
                try
                {
                    Console.WriteLine($"Updating user with id: {id}");
                    var fixedDto = dto with { Id = id };
                    await mediator.Send(new Update.Command(fixedDto), ct);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR in UpdateUser: {ex.Message}");
                    return Results.Problem(
                        title: "Server error",
                        detail: ex.Message,
                        statusCode: 500
                    );
                }
            })
           // .RequirePermission("Users", "Edit")
            .WithName("UpdateUser");

        users.MapDelete("/{id:int}",
            async (IMediator mediator, int id, CancellationToken ct) =>
            {
                try
                {
                    Console.WriteLine($"Deleting user with id: {id}");
                    var result = await mediator.Send(new Delete.Command(id), ct);
                    return result ? Results.NoContent() : Results.NotFound();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR in DeleteUser: {ex.Message}");
                    return Results.Problem(
                        title: "Server error",
                        detail: ex.Message,
                        statusCode: 500
                    );
                }
            })
           // .RequirePermission("Users", "Delete")
            .WithName("DeleteUser");

        return routes;
    }
}