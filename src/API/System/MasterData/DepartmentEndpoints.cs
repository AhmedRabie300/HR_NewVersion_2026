// API/System/MasterData/DepartmentEndpoints.cs
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

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListDepartments.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllDepartments");

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedDepartments.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedDepartments");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetDepartmentById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetDepartmentById");

  
            // POST create
            group.MapPost("/", async (IMediator mediator, CreateDepartmentDto dto, CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateDepartment.Command(dto), ct);
                return Results.Created($"/master-data/departments/{id}", new { id });
            })
            .WithName("CreateDepartment");

            // PUT update
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
            .WithName("UpdateDepartment");

      
            return routes;
        }
    }
}