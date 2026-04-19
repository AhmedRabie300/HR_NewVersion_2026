using Application.Common.Abstractions;
using Application.System.MasterData.Country.Commands;
using Application.System.MasterData.Country.Dtos;
using Application.System.MasterData.Country.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class CountryEndpoints
    {
        public static IEndpointRouteBuilder MapCountryEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/countries")
                .WithTags("Countries");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListCountries.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllCountries");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedCountries.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedCountries");
 

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetCountryById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetCountryById");
 
            group.MapPost("/", async (
                IMediator mediator,
                CreateCountryDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateCountry.Command( dto), ct);
                return Results.Created($"/master-data/countries/{id}", new { id });
            })
            .WithName("CreateCountry");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateCountryDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateCountry.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateCountry");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteCountry.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteCountry");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
               await mediator.Send(new DeleteCountry.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("DeleteCountry");

            return routes;
        }
    }
}