// API/System/MasterData/LocationEndpoints.cs
using API.Helpers;
using Application.System.MasterData.Location.Commands;
using Application.System.MasterData.Location.Dtos;
using Application.System.MasterData.Location.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class LocationEndpoints
    {
        public static IEndpointRouteBuilder MapLocationEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/locations")
                .WithTags("Locations");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListLocations.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllLocations");

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedLocations.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedLocations");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetLocationById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetLocationById");

            // POST create
            group.MapPost("/", async (IMediator mediator, CreateLocationDto dto, CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateLocation.Command(dto), ct);
                return Results.Created($"/master-data/locations/{id}", new { id });
            })
            .WithName("CreateLocation");

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateLocationDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateLocation.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateLocation");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteLocation.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteLocation");

            return routes;
        }
    }
}