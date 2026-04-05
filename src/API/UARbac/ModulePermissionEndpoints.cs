using Application.UARbac.ModulePermissions.Commands;
using Application.UARbac.ModulePermissions.Dtos;
using Application.UARbac.ModulePermissions.Queries;
using MediatR;

namespace API.Endpoints
{
    public static class ModulePermissionEndpoints
    {
        public static IEndpointRouteBuilder MapModulePermissionEndpoints(this IEndpointRouteBuilder routes)
        {
            var permissions = routes.MapGroup("/module-permissions")
                .WithTags("ModulePermissions");

 
             permissions.MapGet("/user/{userId:int}", async (
                IMediator mediator,
                int userId,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetUserModulePermissions.Query(userId), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("frmModules", "View")
            .WithName("GetUserModulePermissions");

             permissions.MapGet("/module/{moduleId:int}", async (
                IMediator mediator,
                int moduleId,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetModulePermissionsByModule.Query(moduleId), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("frmModules", "View")
            .WithName("GetModulePermissionsByModule");

             permissions.MapGet("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetModulePermissionById.Query(id), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("frmModules", "View")
            .WithName("GetModulePermissionById");

 
             permissions.MapPost("/", async (
                IMediator mediator,
                CreateModulePermissionDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateModulePermission.Command(dto), ct);
                return Results.Created($"/module-permissions/{id}", new { id });
            })
           // .RequirePermission("frmModules", "Add")
            .WithName("CreateModulePermission");

 
             permissions.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateModulePermissionDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateModulePermission.Command(fixedDto), ct);
                return Results.NoContent();
            })
           // .RequirePermission("frmModules", "Edit")
            .WithName("UpdateModulePermission");

 
             permissions.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteModulePermission.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
           // .RequirePermission("frmModules", "Delete")
            .WithName("DeleteModulePermission");

      

            return routes;
        }
    }
}