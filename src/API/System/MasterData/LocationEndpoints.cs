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

            // GET /api/hr/master-data/locations?lang=1
            group.MapGet("/", async (
                IMediator mediator,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new ListLocations.Query(lang), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Locations", "View")
            .WithName("GetAllLocations")
            .WithOpenApi();

            // GET /api/hr/master-data/locations/paged?pageNumber=1&pageSize=20&searchTerm=&companyId=&branchId=&lang=1
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                int? companyId = null,
                int? branchId = null,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedLocations.Query(pageNumber, pageSize, searchTerm, companyId, branchId, lang),
                    ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Locations", "View")
            .WithName("GetPagedLocations")
            .WithOpenApi();

            // GET /api/hr/master-data/locations/{id}?lang=1
            group.MapGet("/{id:int}", async (
                IMediator mediator,
                int id,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new GetLocationById.Query(id, lang), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Locations", "View")
            .WithName("GetLocationById")
            .WithOpenApi();

      

            // GET /api/hr/master-data/locations/by-company/{companyId}?lang=1
            group.MapGet("/by-company/{companyId:int}", async (
                IMediator mediator,
                int companyId,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new GetLocationsByCompany.Query(companyId, lang), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Locations", "View")
            .WithName("GetLocationsByCompany")
            .WithOpenApi();

            // POST /api/hr/master-data/locations?lang=1
            group.MapPost("/", async (
                IMediator mediator,
                CreateLocationDto dto,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var id = await mediator.Send(new CreateLocation.Command(dto, lang), ct);
                return Results.Created($"/api/hr/master-data/locations/{id}", new { id });
            })
           // .RequirePermission("Locations", "Add")
            .WithName("CreateLocation")
            .WithOpenApi();

            // PUT /api/hr/master-data/locations/{id}?lang=1
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateLocationDto dto,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateLocation.Command(fixedDto, lang), ct);
                return Results.NoContent();
            })
           // .RequirePermission("Locations", "Edit")
            .WithName("UpdateLocation")
            .WithOpenApi();

            // DELETE /api/hr/master-data/locations/{id}?regUserId=&lang=1 (Soft Delete)
            group.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                int? regUserId,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                await mediator.Send(new SoftDeleteLocation.Command(id, regUserId, lang), ct);
                return Results.NoContent();
            })
           // .RequirePermission("Locations", "Delete")
            .WithName("SoftDeleteLocation")
            .WithOpenApi();

            return routes;
        }
    }
}