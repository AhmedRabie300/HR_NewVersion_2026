using Application.Common.Abstractions;
using Application.System.HRS.Basics.ContractsTypes.Commands;
using Application.System.HRS.Basics.ContractsTypes.Dtos;
using Application.System.HRS.Basics.ContractsTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS.Basics.ContractsTypes
{
    public static class ContractsTypeEndpoints
    {
        public static IEndpointRouteBuilder MapContractsTypeEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/basics/contracts-types")
                .WithTags("ContractsTypes");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListContractsTypes.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllContractsTypes")
            .WithOpenApi();

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedContractsTypes.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedContractsTypes")
            .WithOpenApi();

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetContractsTypeById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetContractsTypeById")
            .WithOpenApi();

            group.MapPost("/", async (
                IMediator mediator,
                CreateContractsTypeDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateContractsType.Command(dto), ct);
                return Results.Created($"/basics/contracts-types/{id}", new { id });
            })
            .WithName("CreateContractsType")
            .WithOpenApi();

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateContractsTypeDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateContractsType.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateContractsType")
            .WithOpenApi();

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                [FromQuery] int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteContractsType.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteContractsType")
            .WithOpenApi();

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteContractsType.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteContractsType")
            .WithOpenApi();

            return routes;
        }
    }
}