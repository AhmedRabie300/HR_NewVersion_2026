using Application.Common.Abstractions;
using Application.System.HRS.VacationsType.Commands;
using Application.System.HRS.VacationsType.Dtos;
using Application.System.HRS.VacationsType.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS
{
    public static class VacationsTypeEndpoints
    {
        public static IEndpointRouteBuilder MapVacationsTypeEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/hrs/vacations-types")
                .WithTags("Vacations Types");

            // ✅ GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListVacationsTypes.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllVacationsTypes");

            // ✅ GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedVacationsTypes.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedVacationsTypes");

            // ✅ GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetVacationsTypeById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetVacationsTypeById");

            // ✅ POST create - بدون CompanyId وبدون RegUserId (زي BankEndpoints)
            group.MapPost("/", async (
                IMediator mediator,
                CreateVacationsTypeDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateVacationsType.Command(dto), ct);
                return Results.Created($"/hrs/vacations-types/{id}", new { id });
            })
            .WithName("CreateVacationsType");

            // ✅ PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateVacationsTypeDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateVacationsType.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateVacationsType");

             group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteVacationsType.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteVacationsType");

             group.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new DeleteVacationsType.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("DeleteVacationsType");

            return routes;
        }
    }
}