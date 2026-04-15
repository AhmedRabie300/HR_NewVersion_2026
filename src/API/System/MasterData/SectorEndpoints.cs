using Application.Common.Abstractions;
using Application.System.MasterData.Sector.Commands;
using Application.System.MasterData.Sector.Dtos;
using Application.System.MasterData.Sector.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class SectorEndpoints
    {
        public static IEndpointRouteBuilder MapSectorEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/sectors")
                .WithTags("Sectors");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListSectors.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllSectors");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedSectors.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedSectors");

            group.MapGet("/by-company", async (
                IMediator mediator,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetSectorsByCompany.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetSectorsByCompany");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetSectorById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetSectorById");

            group.MapPost("/", async (
                IMediator mediator,
                [FromHeader(Name = "CompanyId")] int companyId,
                [FromServices] IContextService contextService,
                CreateSectorDto dto,
                CancellationToken ct) =>
            {
                var regUserId = contextService.GetCurrentUserId();
                var id = await mediator.Send(new CreateSector.Command(companyId, regUserId, dto), ct);
                return Results.Created($"/master-data/sectors/{id}", new { id });
            })
            .WithName("CreateSector");

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