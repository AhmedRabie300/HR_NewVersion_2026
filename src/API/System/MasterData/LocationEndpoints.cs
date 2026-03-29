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
            var group = routes.MapGroup("/api/hr/master-data/locations")
                .WithTags("Locations");

            // GET /api/hr/master-data/locations
            group.MapGet("/", async (
                IMediator mediator,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new ListLocations.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllLocations");

            // GET /api/hr/master-data/locations/paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                int? companyId = null,
                int? branchId = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedLocations.Query(pageNumber, pageSize, searchTerm, companyId, branchId),
                    ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedLocations");

            // GET /api/hr/master-data/locations/{id}
            group.MapGet("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new GetLocationById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetLocationById");

            // GET /api/hr/master-data/locations/by-company/{companyId}
            group.MapGet("/by-company/{companyId:int}", async (
                IMediator mediator,
                int companyId,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new GetLocationsByCompany.Query(companyId), ct);
                return Results.Ok(result);
            })
            .WithName("GetLocationsByCompany");

            // POST /api/hr/master-data/locations
            group.MapPost("/", async (
                IMediator mediator,
                CreateLocationDto dto,
                CancellationToken ct = default) =>
            {
                var id = await mediator.Send(new CreateLocation.Command(dto), ct);
                return Results.Created($"/api/hr/master-data/locations/{id}", new { id });
            })
            .WithName("CreateLocation");

            // PUT /api/hr/master-data/locations/{id}
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateLocationDto dto,
                CancellationToken ct = default) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateLocation.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateLocation");

            // DELETE /api/hr/master-data/locations/{id} (Soft Delete)
            group.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct = default) =>
            {
                await mediator.Send(new SoftDeleteLocation.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteLocation");

            return routes;
        }
    }
}