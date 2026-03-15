// API/Endpoints/LoginEndpoints.cs
using Application.UARbac.Login.Commands;
using Application.UARbac.Login.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

public static class LoginEndpoints
{
    public static IEndpointRouteBuilder MapLoginEndpoints(this IEndpointRouteBuilder routes)
    {
        // Group for authentication endpoints
        var auth = routes.MapGroup("/api/auth").WithTags("Authentication");

        // POST /api/auth/login
        auth.MapPost("/login",
            async (IMediator mediator, LoginRequestDto request, [FromQuery] int lang, CancellationToken ct) =>
            {
                var result = await mediator.Send(new Login.Command(request, lang), ct);

                return result.Success
                    ? Results.Ok(result)
                    : Results.Unauthorized();
            })
            .AllowAnonymous()
            .WithName("Login")
            .WithDescription("Authenticate user and get access token")
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        // GET /api/auth/status
        auth.MapGet("/status",
            () => Results.Ok(new
            {
                Status = "API is running",
                Timestamp = DateTime.UtcNow,
                Version = "1.0"
            }))
            .AllowAnonymous()
            .WithName("Status")
            .WithDescription("Check API health status");

        // POST /api/auth/logout
        auth.MapPost("/logout",
            () => Results.Ok(new
            {
                Message = "Logged out successfully",
                Timestamp = DateTime.UtcNow
            }))
            .AllowAnonymous()
            .WithName("Logout")
            .WithDescription("Logout user (client should discard token)");

        return routes;
    }
}