using Application.Common.Abstractions;
using Application.System.MasterData.MaritalStatus.Commands;
using Application.System.MasterData.MaritalStatus.Dtos;
using Application.System.MasterData.MaritalStatus.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class MaritalStatusEndpoints
    {
        public static IEndpointRouteBuilder MapMaritalStatusEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/marital-statuses")
                .WithTags("MaritalStatuses");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListMaritalStatuses.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllMaritalStatuses");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedMaritalStatuses.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedMaritalStatuses");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetMaritalStatusById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetMaritalStatusById");

            group.MapPost("/", async (
                IMediator mediator,
                CreateMaritalStatusDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateMaritalStatus.Command(dto), ct);
                return Results.Created($"/master-data/marital-statuses/{id}", new { id });
            })
            .WithName("CreateMaritalStatus");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateMaritalStatusDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateMaritalStatus.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateMaritalStatus");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteMaritalStatus.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteMaritalStatus");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
               await mediator.Send(new DeleteMaritalStatus.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("DeleteMaritalStatus");

            return routes;
        }
    }
}