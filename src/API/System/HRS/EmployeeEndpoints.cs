using Application.Common.Abstractions;
using Application.System.HRS.Basics.Employees.Commands;
using Application.System.HRS.Basics.Employees.Queries;
using Application.System.HRS.Employees.Dtos;
using Application.System.HRS.Employees.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
 
namespace API.System.HRS.Employees
{
    public static class EmployeeEndpoints
    {
        public static IEndpointRouteBuilder MapEmployeeEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/hrs/employees")
                .WithTags("Employees");

            // GET all (summary list)
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListEmployees.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllEmployees")
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
                    new GetPagedEmployees.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedEmployees")
            ;

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetEmployeeById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetEmployeeById")
            ;

            // GET by code
            group.MapGet("/by-code/{code}", async (IMediator mediator, string code, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetEmployeeByCode.Query(code), ct);
                return Results.Ok(result);
            })
            .WithName("GetEmployeeByCode")
            ;

            // GET by SSN
            group.MapGet("/by-ssn/{ssnNo}", async (IMediator mediator, string ssnNo, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetEmployeeBySSnNo.Query(ssnNo), ct);
                return Results.Ok(result);
            })
            .WithName("GetEmployeeBySSnNo")
            ;

            // POST create
           group.MapPost("/", async (
                IMediator mediator,
                CreateEmployeeDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateEmployee.Command(dto), ct);
                return Results.Created($"/hrs/employees/{id}", new { id });
            })
            .WithName("CreateEmployee")
            ;

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateEmployeeDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateEmployee.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateEmployee")
            ;

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                [FromQuery] int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteEmployee.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteEmployee")
            ;

            // DELETE hard
            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteEmployee.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteEmployee");

           

            group.MapGet("/list", async (
                IMediator mediator,
                HttpContext httpContext,
                CancellationToken ct) =>
            {
                var pageNumber = int.Parse(httpContext.Request.Query["pageNumber"].FirstOrDefault() ?? "1");
                var pageSize = int.Parse(httpContext.Request.Query["pageSize"].FirstOrDefault() ?? "20");
                var orderBy = httpContext.Request.Query["orderBy"].FirstOrDefault();
                var orderDirection = httpContext.Request.Query["orderDirection"].FirstOrDefault();

                 var filters = httpContext.Request.Query
                    .Where(x => x.Key != "pageNumber"
                                && x.Key != "pageSize"
                                && x.Key != "orderBy"
                                && x.Key != "orderDirection")
                    .ToDictionary(x => x.Key.ToLower(), x => x.Value.ToString());

                var result = await mediator.Send(new GetEmployeeList.Query(
                    pageNumber, pageSize, orderBy, orderDirection, filters), ct);

                return Results.Json(result);
            })
            .WithName("GetEmployeeList");


            return routes;
        }
    }
}