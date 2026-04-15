using Application.Common.Abstractions;
using Application.System.HRS.VacationsPaidType.Commands;
using Application.System.HRS.VacationsPaidType.Dtos;
using Application.System.HRS.VacationsPaidType.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS
{
    public static class VacationsPaidTypeEndpoints
    {
        public static IEndpointRouteBuilder MapVacationsPaidTypeEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/hrs/vacations-paid-types")
                .WithTags("Vacations Paid Types");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListVacationsPaidTypes.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllVacationsPaidTypes");

            // ✅ إضافة Paged Endpoint
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedVacationsPaidTypes.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedVacationsPaidTypes");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetVacationsPaidTypeById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetVacationsPaidTypeById");

            group.MapPost("/", async (
                IMediator mediator,
                [FromHeader(Name = "CompanyId")] int companyId,
                [FromServices] IContextService contextService,
                CreateVacationsPaidTypeDto dto,
                CancellationToken ct) =>
            {
                var regUserId = contextService.GetCurrentUserId();
                var id = await mediator.Send(new CreateVacationsPaidType.Command(companyId, regUserId, dto), ct);
                return Results.Created($"/hrs/vacations-paid-types/{id}", new { id });
            })
            .WithName("CreateVacationsPaidType");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateVacationsPaidTypeDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateVacationsPaidType.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateVacationsPaidType");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteVacationsPaidType.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteVacationsPaidType");

            group.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteVacationsPaidType.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteVacationsPaidType");

            return routes;
        }
    }
}