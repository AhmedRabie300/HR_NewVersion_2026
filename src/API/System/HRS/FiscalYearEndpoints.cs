using Application.Common.Abstractions;
using Application.System.HRS.Basics.FiscalYears.Commands;
using Application.System.HRS.Basics.FiscalYears.Dtos;
using Application.System.HRS.Basics.FiscalYears.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS
{
    public static class FiscalYearEndpoints
    {
        public static IEndpointRouteBuilder MapFiscalYearEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/basics/fiscal-years")
                .WithTags("FiscalYears");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListFiscalYears.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllFiscalYears")
            ;

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedFiscalYears.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedFiscalYears")
            ;

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetFiscalYearById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetFiscalYearById")
            ;

            // POST create
            group.MapPost("/", async (
                IMediator mediator,
                CreateFiscalYearDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateFiscalYear.Command(dto), ct);
                return Results.Created($"/basics/fiscal-years/{id}", new { id });
            })
            .WithName("CreateFiscalYear")
            ;

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateFiscalYearDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateFiscalYear.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateFiscalYear")
            ;

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                [FromQuery] int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteFiscalYear.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteFiscalYear")
            ;

            // DELETE hard
            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteFiscalYear.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteFiscalYear")
            ;

            return routes;
        }
    }
}