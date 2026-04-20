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

            // ✅ GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListVacationsPaidTypes.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllVacationsPaidTypes");

            // ✅ GET paged
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

            // ✅ GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetVacationsPaidTypeById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetVacationsPaidTypeById");

            // ✅ POST create - بدون CompanyId وبدون RegUserId (زي BankEndpoints)
            group.MapPost("/", async (
                IMediator mediator,
                CreateVacationsPaidTypeDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateVacationsPaidType.Command(dto), ct);
                return Results.Created($"/hrs/vacations-paid-types/{id}", new { id });
            })
            .WithName("CreateVacationsPaidType");

            // ✅ PUT update
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

            // ✅ DELETE soft - بدون regUserId (زي BankEndpoints)
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteVacationsPaidType.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteVacationsPaidType");

            // ✅ DELETE hard
            group.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new DeleteVacationsPaidType.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("DeleteVacationsPaidType");

            return routes;
        }
    }
}