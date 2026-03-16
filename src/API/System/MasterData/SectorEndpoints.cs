using API.Helpers;
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
            var group = routes.MapGroup("/api/hr/master-data/sectors")
                .WithTags("Sectors");

            // GET /api/hr/master-data/sectors?lang=1
            group.MapGet("/", async (
                IMediator mediator,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new ListSectors.Query(lang), ct);
                return Results.Ok(result);
            })
            .RequirePermission("Sectors", "View")
            .WithName("GetAllSectors")
            .WithOpenApi();

            // GET /api/hr/master-data/sectors/paged?pageNumber=1&pageSize=20&searchTerm=&companyId=&lang=1
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                int? companyId = null,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedSectors.Query(pageNumber, pageSize, searchTerm, companyId, lang),
                    ct);
                return Results.Ok(result);
            })
            .RequirePermission("Sectors", "View")
            .WithName("GetPagedSectors")
            .WithOpenApi();

            // GET /api/hr/master-data/sectors/{id}?lang=1
            group.MapGet("/{id:int}", async (
                IMediator mediator,
                int id,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new GetSectorById.Query(id, lang), ct);
                return Results.Ok(result);
            })
            .RequirePermission("Sectors", "View")
            .WithName("GetSectorById")
            .WithOpenApi();

    

            // GET /api/hr/master-data/sectors/by-company/{companyId}?lang=1
            group.MapGet("/by-company/{companyId:int}", async (
                IMediator mediator,
                int companyId,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new GetSectorsByCompany.Query(companyId, lang), ct);
                return Results.Ok(result);
            })
            .RequirePermission("Sectors", "View")
            .WithName("GetSectorsByCompany")
            .WithOpenApi();

            // POST /api/hr/master-data/sectors?lang=1
            group.MapPost("/", async (
                IMediator mediator,
                CreateSectorDto dto,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var id = await mediator.Send(new CreateSector.Command(dto, lang), ct);
                return Results.Created($"/api/hr/master-data/sectors/{id}", new { id });
            })
            .RequirePermission("Sectors", "Add")
            .WithName("CreateSector")
            .WithOpenApi();

            // PUT /api/hr/master-data/sectors/{id}?lang=1
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateSectorDto dto,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateSector.Command(fixedDto, lang), ct);
                return Results.NoContent();
            })
            .RequirePermission("Sectors", "Edit")
            .WithName("UpdateSector")
            .WithOpenApi();

            // DELETE /api/hr/master-data/sectors/{id}?regUserId=&lang=1 (Soft Delete)
            group.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                int? regUserId,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                await mediator.Send(new SoftDeleteSector.Command(id, regUserId, lang), ct);
                return Results.NoContent();
            })
            .RequirePermission("Sectors", "Delete")
            .WithName("SoftDeleteSector")
            .WithOpenApi();

            return routes;
        }
    }
}