using Application.System.MasterData.Nationality.Commands;
using Application.System.MasterData.Nationality.Dtos;
using Application.System.MasterData.Nationality.Queries;
using Application.Common.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class NationalityEndpoints
    {
        public static IEndpointRouteBuilder MapNationalityEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/nationalities")
                .WithTags("Nationalities");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListNationalities.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllNationalities");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedNationalities.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedNationalities");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetNationalityById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetNationalityById");

            group.MapPost("/", async (
                IMediator mediator,
                CreateNationalityDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateNationality.Command( dto), ct);
                return Results.Created($"/master-data/nationalities/{id}", new { id });
            })
            .WithName("CreateNationality");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateNationalityDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateNationality.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateNationality");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteNationality.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteNationality");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                await mediator.Send(new DeleteNationality.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("DeleteNationality");

            return routes;
        }
    }
}