// API/Endpoints/PermissionEndpoints.cs
using Application.UARbac.FormPermission.Dtos;
using Application.UARbac.FormPermission.Queries;
using Application.UARbac.FormPermissions.Commands;
 
using MediatR;

namespace API.Endpoints
{
    public static class FormPermissionEndpoints
    {
        public static IEndpointRouteBuilder MapPermissionEndpoints(this IEndpointRouteBuilder routes)
        {
            var permissions = routes.MapGroup("/permissions").WithTags("Permissions");

            // ========== USER PERMISSIONS ==========
            // Get user permissions
            permissions.MapGet("/users/{userId:int}",
                async (IMediator mediator, int userId, CancellationToken ct) =>
                {
                    var result = await mediator.Send(new GetUserPermissions.Query(userId), ct);
                    return Results.Ok(result);
                })
                .WithName("GetUserPermissions");
 
           
            // Create permission
            permissions.MapPost("/",
                async (IMediator mediator, CreateFormPermissionDto dto, CancellationToken ct) =>
                {
                    var id = await mediator.Send(new CreateFormPermission.Command(dto), ct);
                    return Results.Created($"/permissions/{id}", new { id });
                })
                .WithName("CreatePermission");

     
            return routes;
        }
    }
}