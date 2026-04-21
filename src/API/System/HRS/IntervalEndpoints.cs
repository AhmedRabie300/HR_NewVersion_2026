using Application.Common.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.Interval.Commands;
using Application.System.HRS.Basics.FiscalTransactions.Interval.Dtos;
using Application.System.HRS.Basics.FiscalTransactions.Interval.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS.Basics.FiscalTransactions
{
    public static class IntervalEndpoints
    {
        public static IEndpointRouteBuilder MapIntervalEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/basics/fiscal-transactions/intervals")
                .WithTags("Intervals");

             group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListIntervals.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllIntervals");

             group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedIntervals.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedIntervals");

             group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetIntervalById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetIntervalById");

             group.MapPost("/", async (
                IMediator mediator,
                CreateIntervalDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateInterval.Command(dto), ct);
                return Results.Created($"/basics/fiscal-transactions/intervals/{id}", new { id });
            })
            .WithName("CreateInterval");

             group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateIntervalDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateInterval.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateInterval");

             group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteInterval.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteInterval");

             group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteInterval.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteInterval");

            return routes;
        }
    }
}