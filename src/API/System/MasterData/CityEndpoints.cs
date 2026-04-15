using Application.Common.Abstractions;
using Application.System.MasterData.City.Commands;
using Application.System.MasterData.City.Dtos;
using Application.System.MasterData.City.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class CityEndpoints
    {
        public static IEndpointRouteBuilder MapCityEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/cities")
                .WithTags("Cities");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListCities.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllCities");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedCities.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedCities");
 
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetCityById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetCityById");

            group.MapPost("/", async (
                IMediator mediator,
                [FromHeader(Name = "X-Company-Id")] int companyId,
                [FromServices] IContextService contextService,
                CreateCityDto dto,
                CancellationToken ct) =>
            {
                 var regUserId = contextService.GetCurrentUserId();

                var id = await mediator.Send(new CreateCity.Command(companyId, regUserId, dto), ct);
                return Results.Created($"/master-data/cities/{id}", new { id });
            })
            .WithName("CreateCity");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateCityDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateCity.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateCity");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteCity.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteCity");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteCity.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteCity");

            return routes;
        }
    }
}