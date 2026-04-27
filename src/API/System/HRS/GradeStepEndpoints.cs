using Application.Common.Abstractions;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Commands;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Queries;
using Application.System.HRS.Basics.Grades.Commands;
using Application.System.HRS.Basics.Grades.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS
{
    public static class GradeStepEndpoints
    {
        public static IEndpointRouteBuilder MapGradeStepEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/basics/grades/steps")
                .WithTags("GradeSteps");

            // ==================== GradeStep (Master) ====================

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListGradeSteps.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllGradeSteps");


            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                int? gradeId = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedGradeSteps.Query(pageNumber, pageSize, searchTerm, gradeId), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedGradeSteps");
            

            // GET by grade id
            group.MapGet("/by-grade/{gradeId:int}", async (
                IMediator mediator,
                int gradeId,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetGradeStepsByGradeId.Query(gradeId), ct);
                return Results.Ok(result);
            })
            .WithName("GetGradeStepsByGradeId")
            ;

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetGradeStepById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetGradeStepById")
            ;

            // POST create
            group.MapPost("/", async (
                IMediator mediator,
                CreateGradeStepDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateGradeStep.Command(dto), ct);
                return Results.Created($"/basics/grades/steps/{id}", new { id });
            })
            .WithName("CreateGradeStep")
            ;

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateGradeStepDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateGradeStep.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateGradeStep")
            ;

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                [FromQuery] int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteGradeStep.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteGradeStep")
            ;

            // DELETE hard
            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteGradeStep.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteGradeStep")
            ;

            // ==================== GradeStepTransaction (Detail) ====================

            // GET transactions by grade step id
            group.MapGet("/{gradeStepId:int}/transactions", async (
                IMediator mediator,
                int gradeStepId,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetGradeStepTransactions.Query(gradeStepId), ct);
                return Results.Ok(result);
            })
            .WithName("GetGradeStepTransactions")
            ;

            // POST add transaction
            group.MapPost("/{gradeStepId:int}/transactions", async (
                IMediator mediator,
                int gradeStepId,
                CreateGradeStepTransactionDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new AddGradeStepTransaction.Command(gradeStepId, dto), ct);
                return Results.Created($"/basics/grades/steps/{gradeStepId}/transactions/{id}", new { id });
            })
            .WithName("AddGradeStepTransaction")
            ;

            // PUT update transaction
            group.MapPut("/transactions/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateGradeStepTransactionDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateGradeStepTransaction.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateGradeStepTransaction")
            ;

            // DELETE soft transaction
            group.MapDelete("/transactions/{id:int}/soft", async (
                IMediator mediator,
                int id,
                [FromQuery] int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteGradeStepTransaction.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteGradeStepTransaction")
            ;

            // DELETE hard transaction
            group.MapDelete("/transactions/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteGradeStepTransaction.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteGradeStepTransaction")
            ;

            return routes;
        }
    }
}