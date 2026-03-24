using API.Helpers;
using Application.System.MasterData.BloodGroup.Commands;
using Application.System.MasterData.BloodGroup.Dtos;
using Application.System.MasterData.BloodGroup.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class BloodGroupEndpoints
    {
        public static IEndpointRouteBuilder MapBloodGroupEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/hr/master-data/blood-groups")
                .WithTags("BloodGroups");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListBloodGroups.Query(), ct);
                return Results.Ok(result);
            })
            .RequirePermission("BloodGroups", "View")
            .WithName("GetAllBloodGroups");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedBloodGroups.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .RequirePermission("BloodGroups", "View")
            .WithName("GetPagedBloodGroups");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetBloodGroupById.Query(id), ct);
                return Results.Ok(result);
            })
            .RequirePermission("BloodGroups", "View")
            .WithName("GetBloodGroupById");

            group.MapPost("/", async (IMediator mediator, CreateBloodGroupDto dto, CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateBloodGroup.Command(dto), ct);
                return Results.Created($"/api/hr/master-data/blood-groups/{id}", new { id });
            })
            .RequirePermission("BloodGroups", "Add")
            .WithName("CreateBloodGroup");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateBloodGroupDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateBloodGroup.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .RequirePermission("BloodGroups", "Edit")
            .WithName("UpdateBloodGroup");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteBloodGroup.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .RequirePermission("BloodGroups", "Delete")
            .WithName("DeleteBloodGroup");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteBloodGroup.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .RequirePermission("BloodGroups", "Delete")
            .WithName("SoftDeleteBloodGroup");

            return routes;
        }
    }
}