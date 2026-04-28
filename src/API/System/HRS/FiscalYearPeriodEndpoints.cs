using Application.Common.Abstractions;
using Application.System.HRS.Basics.FiscalPeriod.Commands;
using Application.System.HRS.Basics.FiscalPeriod.Dtos;
using Application.System.HRS.Basics.FiscalPeriod.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS.Basics.FiscalPeriod
{
    public static class FiscalYearPeriodEndpoints
    {
        public static IEndpointRouteBuilder MapFiscalYearPeriodEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/basics/fiscal-periods")
                .WithTags("FiscalYearPeriods");

 
            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListFiscalYearPeriods.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllFiscalYearPeriods")
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
                    new GetPagedFiscalYearPeriods.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedFiscalYearPeriods")
            ;

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetFiscalYearPeriodById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetFiscalYearPeriodById")
            ;

            // GET by fiscal year id
            group.MapGet("/by-fiscal-year/{fiscalYearId:int}", async (IMediator mediator, int fiscalYearId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetFiscalYearPeriodsByFiscalYearId.Query(fiscalYearId), ct);
                return Results.Ok(result);
            })
            .WithName("GetFiscalYearPeriodsByFiscalYearId")
            ;

            // POST generate periods
            group.MapPost("/generate", async (
                IMediator mediator,
                GenerateFiscalYearPeriodsDto dto,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GenerateFiscalYearPeriods.Command(dto.FiscalYearId, dto.IsFormal, dto.SourceFiscalYearId), ct);
                return Results.Ok(new { generatedCount = result });
            })
            .WithName("GenerateFiscalYearPeriods")
            ;

            // POST create
            group.MapPost("/", async (
                IMediator mediator,
                CreateFiscalYearPeriodDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateFiscalYearPeriod.Command(dto), ct);
                return Results.Created($"/basics/fiscal-periods/{id}", new { id });
            })
            .WithName("CreateFiscalYearPeriod")
            ;

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateFiscalYearPeriodDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateFiscalYearPeriod.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateFiscalYearPeriod")
            ;

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                [FromQuery] int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteFiscalYearPeriod.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteFiscalYearPeriod")
            ;

            // DELETE hard
            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteFiscalYearPeriod.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteFiscalYearPeriod")
            ;

            // ==================== FiscalYearPeriodModule (Detail 2) ====================

            // GET modules by period id
            group.MapGet("/{periodId:int}/modules", async (IMediator mediator, int periodId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetFiscalYearPeriodModules.Query(periodId), ct);
                return Results.Ok(result);
            })
            .WithName("GetFiscalYearPeriodModules")
            ;

            // POST add module
            group.MapPost("/modules", async (
                IMediator mediator,
                CreateFiscalYearPeriodModuleDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new AddFiscalYearPeriodModule.Command(dto), ct);
                return Results.Created($"/basics/fiscal-periods/modules/{id}", new { id });
            })
            .WithName("AddFiscalYearPeriodModule")
            ;

            // PUT update module
            group.MapPut("/modules/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateFiscalYearPeriodModuleDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateFiscalYearPeriodModule.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateFiscalYearPeriodModule")
            ;

            // DELETE soft module
            group.MapDelete("/modules/{id:int}/soft", async (
                IMediator mediator,
                int id,
                [FromQuery] int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteFiscalYearPeriodModule.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteFiscalYearPeriodModule")
            ;

            // DELETE hard module
            group.MapDelete("/modules/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteFiscalYearPeriodModule.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteFiscalYearPeriodModule")
            ;

            return routes;
        }
    }
}