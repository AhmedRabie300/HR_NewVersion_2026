using Application.Common.Abstractions;
using Application.System.MasterData.Region.Commands;
using Application.System.MasterData.Region.Dtos;
using Application.System.MasterData.Region.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class RegionEndpoints
    {
        public static IEndpointRouteBuilder MapRegionEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/regions")
                .WithTags("Regions");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListRegions.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllRegions");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedRegions.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedRegions");

            group.MapGet("/by-country/{countryId:int}", async (IMediator mediator, int countryId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetRegionsByCountryId.Query(countryId), ct);
                return Results.Ok(result);
            })
            .WithName("GetRegionsByCountryId");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetRegionById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetRegionById");

            group.MapPost("/", async (
                IMediator mediator,
                CreateRegionDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateRegion.Command(dto), ct);
                return Results.Created($"/master-data/regions/{id}", new { id });
            })
            .WithName("CreateRegion");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateRegionDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateRegion.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateRegion");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteRegion.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteRegion");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                await mediator.Send(new DeleteRegion.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("DeleteRegion");

            return routes;
        }
    }
}