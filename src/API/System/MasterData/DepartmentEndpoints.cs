using Application.Common.Abstractions;
using Application.System.MasterData.Department.Commands;
using Application.System.MasterData.Department.Dtos;
using Application.System.MasterData.Department.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class DepartmentEndpoints
    {
        public static IEndpointRouteBuilder MapDepartmentEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/departments")
                .WithTags("Departments");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListDepartments.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllDepartments");

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

            // ✅ تعديل: من غير companyId في الـ URL
            group.MapGet("/by-company", async (
                IMediator mediator,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetDepartmentsByCompany.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetDepartmentsByCompany");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetDepartmentById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetDepartmentById");

            group.MapPost("/", async (
                IMediator mediator,
                [FromHeader(Name = "CompanyId")] int companyId,
                [FromServices] IContextService contextService,
                CreateDepartmentDto dto,
                CancellationToken ct) =>
            {
                var regUserId = contextService.GetCurrentUserId();
                var id = await mediator.Send(new CreateDepartment.Command(companyId, regUserId, dto), ct);
                return Results.Created($"/master-data/departments/{id}", new { id });
            })
            .WithName("CreateDepartment");

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

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteDepartment.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteDepartment");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteDepartment.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteDepartment");

            return routes;
        }
    }
}