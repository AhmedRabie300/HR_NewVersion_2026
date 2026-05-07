using Application.Common.Abstractions;
using Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Commands;
using Application.System.HRS.Basics.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.EmployeesClasses.Queries;
using Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Commands;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS
{
    public static class EmployeeClassEndpoints
    {
        public static IEndpointRouteBuilder MapEmployeeClassEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/hrs/employee-classes")
                .WithTags("EmployeeClasses");

            // ==================== EmployeeClass (Master) ====================

            //// GET all
            //group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            //{
            //    var result = await mediator.Send(new ListEmployeeClasses.Query(), ct);
            //    return Results.Ok(result);
            //})
            //.WithName("GetAllEmployeeClasses");
           

            //// GET paged
            //group.MapGet("/paged", async (
            //    IMediator mediator,
            //    int pageNumber = 1,
            //    int pageSize = 20,
            //    string? searchTerm = null,
            //    CancellationToken ct = default) =>
            //{
            //    var result = await mediator.Send(
            //        new GetPagedEmployeeClasses.Query(pageNumber, pageSize, searchTerm), ct);
            //    return Results.Ok(result);
            //})
            //.WithName("GetPagedEmployeeClasses")
            //;



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
                var result = await mediator.Send(new GetEmployeeClassList.Query(
                    pageNumber, pageSize, orderBy, orderDirection, Filters), ct);
                return Results.Json(result);

            })
            .WithName("GetPagedEmployeeClasses");
            

             group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetEmployeeClassById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetEmployeeClassById")
            ;

             group.MapGet("/{classId:int}/delays", async (IMediator mediator, int classId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetEmployeeClassDelays.Query(classId), ct);
                return Results.Ok(result);
            })
            .WithName("GetEmployeeClassDelays")
            ;

             group.MapGet("/{classId:int}/vacations", async (IMediator mediator, int classId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetEmployeeClassVacations.Query(classId), ct);
                return Results.Ok(result);
            })
            .WithName("GetEmployeeClassVacations")
            ;

             group.MapPost("/", async (
                IMediator mediator,
                CreateEmployeeClassDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateEmployeeClass.Command(dto), ct);
                return Results.Created($"/hrs/employee-classes/{id}", new { id });
            })
            .WithName("CreateEmployeeClass");

            
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateEmployeeClassDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateEmployeeClass.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateEmployeeClass");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteEmployeeClass.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteEmployeeClass")
            ;



            // ==================== EmployeeClassDelay (Detail 1) ====================

            // POST add delay
            //group.MapPost("/{classId:int}/delays", async (
            //    IMediator mediator,
            //    int classId,
            //    CreateEmployeeClassDelayDto dto,
            //    CancellationToken ct) =>
            //{
            //    var id = await mediator.Send(new AddEmployeeClassDelay.Command(classId, dto), ct);
            //    return Results.Created($"/hrs/employee-classes/{classId}/delays/{id}", new { id });
            //})
            //.WithName("AddEmployeeClassDelay");


            //// PUT update delay
            //group.MapPut("/delays/{id:int}", async (
            //    IMediator mediator,
            //    int id,
            //    UpdateEmployeeClassDelayDto dto,
            //    CancellationToken ct) =>
            //{
            //    var fixedDto = dto with { Id = id };
            //    await mediator.Send(new UpdateEmployeeClassDelay.Command(fixedDto), ct);
            //    return Results.NoContent();
            //})
            //.WithName("UpdateEmployeeClassDelay");
            
            // DELETE soft delay
            group.MapDelete("/delays/{id:int}/soft", async (
                IMediator mediator,
                int id,
                 CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteEmployeeClassDelay.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteEmployeeClassDelay");


            //// DELETE hard delay
            //group.MapDelete("/delays/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            //{
            //    var result = await mediator.Send(new DeleteEmployeeClassDelay.Command(id), ct);
            //    return result ? Results.NoContent() : Results.NotFound();
            //})
            //.WithName("DeleteEmployeeClassDelay");


            // ==================== EmployeeClassVacation (Detail 2) ====================

            //// POST add vacation
            //group.MapPost("/{classId:int}/vacations", async (
            //    IMediator mediator,
            //    int classId,
            //    CreateEmployeeClassVacationDto dto,
            //    CancellationToken ct) =>
            //{
            //    var id = await mediator.Send(new AddEmployeeClassVacation.Command(classId, dto), ct);
            //    return Results.Created($"/hrs/employee-classes/{classId}/vacations/{id}", new { id });
            //})
            //.WithName("AddEmployeeClassVacation");


            //// PUT update vacation
            //group.MapPut("/vacations/{id:int}", async (
            //    IMediator mediator,
            //    int id,
            //    UpdateEmployeeClassVacationDto dto,
            //    CancellationToken ct) =>
            //{
            //    var fixedDto = dto with { Id = id };
            //    await mediator.Send(new UpdateEmployeeClassVacation.Command(fixedDto), ct);
            //    return Results.NoContent();
            //})
            //.WithName("UpdateEmployeeClassVacation");


            group.MapDelete("/vacations/{id:int}/soft", async (
               IMediator mediator,
               int id,
                CancellationToken ct) =>
           {
               await mediator.Send(new SoftDeleteEmployeeClassVacation.Command(id), ct);
               return Results.NoContent();
           })
           .WithName("SoftDeleteEmployeeClassVacation");
 
           // group.MapDelete("/vacations/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
           //{
           //    var result = await mediator.Send(new DeleteEmployeeClassVacation.Command(id), ct);
           //    return result ? Results.NoContent() : Results.NotFound();
           //})
           //.WithName("DeleteEmployeeClassVacation");
           

 

            return routes;
        }
    }
}