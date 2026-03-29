using API.Helpers;
using Application.System.MasterData.Religion.Commands;
using Application.System.MasterData.Religion.Dtos;
using Application.System.MasterData.Religion.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class ReligionEndpoints
    {
        public static IEndpointRouteBuilder MapReligionEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/religions")
                .WithTags("Religions");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListReligions.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllReligions");

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedReligions.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedReligions");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetReligionById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetReligionById");

            // POST create
            group.MapPost("/", async (IMediator mediator, CreateReligionDto dto, CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateReligion.Command(dto), ct);
                return Results.Created($"/master-data/religions/{id}", new { id });
            })
            .WithName("CreateReligion");

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateReligionDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateReligion.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateReligion");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteReligion.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteReligion");

            return routes;
        }
    }
}