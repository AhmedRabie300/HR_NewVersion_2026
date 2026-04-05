using Application.System.MasterData.Sector.Commands;
using Application.System.MasterData.Sector.Dtos;
using Application.System.MasterData.Sector.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class SectorEndpoints
    {
        public static IEndpointRouteBuilder MapSectorEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/sectors")
                .WithTags("Sectors");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListSectors.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllSectors");

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                int? companyId = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedSectors.Query(pageNumber, pageSize, searchTerm, companyId), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedSectors");

            // GET by company
            group.MapGet("/by-company/{companyId:int}", async (IMediator mediator, int companyId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetSectorsByCompany.Query(companyId), ct);
                return Results.Ok(result);
            })
            .WithName("GetSectorsByCompany");

       
            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetSectorById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetSectorById");

            // POST create
            group.MapPost("/", async (IMediator mediator, CreateSectorDto dto, CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateSector.Command(dto), ct);
                return Results.Created($"/master-data/sectors/{id}", new { id });
            })
            .WithName("CreateSector");

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateSectorDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateSector.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateSector");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteSector.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteSector");

            return routes;
        }
    }
}