using Application.Common.Abstractions;
using Application.System.HRS.Basics.EmployeesClasses.Grades.Queries;
using Application.System.HRS.Basics.EmployeesClasses.Queries;
using Application.System.HRS.Basics.GradeAndClasses.Grades.Commands;
using Application.System.HRS.Basics.Grades.Commands;
using Application.System.HRS.Basics.Grades.Queries;
using Application.System.HRS.Basics.Grades.Queries;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Commands;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS.Basics.Grades
{
    public static class GradeEndpoints
    {
        public static IEndpointRouteBuilder MapGradeEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/hrs/grades")
                .WithTags("Grades");

            // ==================== Grade (Master) ====================

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListGrades.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllGrades");



            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? orderBy = null,
                string? orderDirection = null,
string? Filters = null,
CancellationToken ct = default) =>

            {


                var result = await mediator.Send(new GetGradeList.Query(
                    pageNumber, pageSize, orderBy, orderDirection, Filters), ct);

                return Results.Json(result);

                 
            })
                .WithName("GetPagedGrades");
 
            //;

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetGradeById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetGradeById")
            ;

            group.MapPost("/", async (
                IMediator mediator,
                CreateGradeDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateGrade.Command(dto), ct);
                return Results.Created($"/hrs/grades/{id}", new { id });
            })
            .WithName("CreateGrade")
            ;

            group.MapPut("/{id:int}/UpdateGrade", async (
        IMediator mediator,
        int id,
        UpdateGradeDto dto,
        CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateGrade.Command(fixedDto), ct);
                return Results.NoContent();
            })

            .WithName("UpdateGrade");
            

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                 CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteGrade.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteGrade")
            ;

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteGrade.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteGrade")
            ;

            // ==================== GradeTransaction (Detail) ====================

            group.MapGet("/{gradeId:int}/transactions", async (IMediator mediator, int gradeId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetGradeTransactions.Query(gradeId), ct);
                return Results.Ok(result);
            })
            .WithName("GetGradeTransactions")
            ;

            group.MapPost("/{gradeId:int}/transactions", async (
                IMediator mediator,
                int gradeId,
                CreateGradeTransactionDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new AddGradeTransaction.Command(gradeId, dto), ct);
                return Results.Created($"/hrs/grades/{gradeId}/transactions/{id}", new { id });
            })
            .WithName("AddGradeTransaction")
            ;
 

            group.MapDelete("/transactions/{id:int}/soft", async (
                IMediator mediator,
                int id,
                 CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteGradeTransaction.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteGradeTransaction")
            ;

            group.MapDelete("/transactions/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteGradeTransaction.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteGradeTransaction")
            ;



         
            return routes;
        }
    }
}