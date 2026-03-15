using API.Helpers;  
using Application.UARbac.Modules.Commands;
using Application.UARbac.Modules.Dtos;
using Application.UARbac.Modules.Queries;
using MediatR;

namespace API.Endpoints
{
    public static class ModuleEndpoints
    {
        public static IEndpointRouteBuilder MapModuleEndpoints(this IEndpointRouteBuilder routes)
        {
             var modules = routes.MapGroup("/modules")
                .WithTags("Modules") 
                .RequireAuthorization();  

 
             modules.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListModules.Query(), ct);
                return Results.Ok(result);
            })
            .RequirePermission("frmModules", "View")  
            .WithName("GetAllModules")
            .WithDescription("Get all modules")
            .Produces<List<GetModuleDto>>(StatusCodes.Status200OK);

          
             modules.MapGet("/active", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetActiveModules.Query(), ct);
                return Results.Ok(result);
            })
            .RequirePermission("frmModules", "View")
            .WithName("GetActiveModules")
            .WithDescription("Get only active modules")
            .Produces<List<GetModuleDto>>(StatusCodes.Status200OK);

             modules.MapGet("/by-type/{type}", async (
                IMediator mediator,
                string type,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetModulesByType.Query(type), ct);
                return Results.Ok(result);
            })
            .RequirePermission("frmModules", "View")
            .WithName("GetModulesByType")
            .WithDescription("Get modules by type (HR, GL, AR, AP, etc)")
            .Produces<List<GetModuleDto>>(StatusCodes.Status200OK);

             modules.MapGet("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetModuleById.Query(id), ct);

                return result != null
                    ? Results.Ok(result)
                    : Results.NotFound(new { message = $"Module with ID {id} not found" });
            })
            .RequirePermission("frmModules", "View")
            .WithName("GetModuleById")
            .WithDescription("Get module by ID")
            .Produces<GetModuleDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

 
             modules.MapPost("/", async (
                IMediator mediator,
                CreateModuleDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateModule.Command(dto), ct);
                return Results.Created($"/modules/{id}", new { id });
            })
            .RequirePermission("frmModules", "Add")
            .WithName("CreateModule")
            .WithDescription("Create new module")
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem();

 
             modules.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateModuleDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateModule.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .RequirePermission("frmModules", "Edit")
            .WithName("UpdateModule")
            .WithDescription("Update existing module")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem();

 
             modules.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteModule.Command(id), ct);
                return result
                    ? Results.NoContent()
                    : Results.NotFound(new { message = $"Module with ID {id} not found" });
            })
            .RequirePermission("frmModules", "Delete")
            .WithName("DeleteModule")
            .WithDescription("Delete module")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

          
            return routes;
        }
    }
}