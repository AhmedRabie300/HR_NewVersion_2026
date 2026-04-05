using API.Helpers;
using Application.System.MasterData.Department.Commands;
using Application.System.MasterData.Department.Dtos;
using Application.System.MasterData.Department.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class DepartmentEndpoints
    {
        public static IEndpointRouteBuilder MapDepartmentEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/departments")
                .WithTags("Departments");

            // GET /api/hr/master-data/departments
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListDepartments.Query(), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Departments", "View")
            .WithName("GetAllDepartments");

            // GET /api/hr/master-data/departments/paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                int? companyId = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedDepartments.Query(pageNumber, pageSize, searchTerm, companyId),
                    ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Departments", "View")
            .WithName("GetPagedDepartments");

            // GET /api/hr/master-data/departments/{id}
            group.MapGet("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetDepartmentById.Query(id), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Departments", "View")
            .WithName("GetDepartmentById");

            

            // GET /api/hr/master-data/departments/by-company/{companyId}
            group.MapGet("/by-company/{companyId:int}", async (
                IMediator mediator,
                int companyId,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetDepartmentsByCompany.Query(companyId), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Departments", "View")
            .WithName("GetDepartmentsByCompany");

            // POST /api/hr/master-data/departments
            group.MapPost("/", async (
                IMediator mediator,
                CreateDepartmentDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateDepartment.Command(dto), ct);
                return Results.Created($"/api/hr/master-data/departments/{id}", new { id });
            })
           // .RequirePermission("Departments", "Add")
            .WithName("CreateDepartment");

            // PUT /api/hr/master-data/departments/{id}
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateDepartmentDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateDepartment.Command(fixedDto), ct);
                return Results.NoContent();
            })
           // .RequirePermission("Departments", "Edit")
            .WithName("UpdateDepartment");

            return routes;
        }
    }
}