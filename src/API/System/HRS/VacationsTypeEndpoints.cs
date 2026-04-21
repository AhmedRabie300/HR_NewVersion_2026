using Application.Common.Abstractions;
using Application.System.HRS.VacationAndEndOfService.VacationsType.Commands;
using Application.System.HRS.VacationAndEndOfService.VacationsType.Dtos;
using Application.System.HRS.VacationAndEndOfService.VacationsType.Queries;
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

             group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListVacationsTypes.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllVacationsTypes");

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

             group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetVacationsTypeById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetVacationsTypeById");

             group.MapPost("/", async (
                IMediator mediator,
                CreateVacationsTypeDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateVacationsType.Command(dto), ct);
                return Results.Created($"/hrs/vacations-types/{id}", new { id });
            })
            .WithName("CreateVacationsType");

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