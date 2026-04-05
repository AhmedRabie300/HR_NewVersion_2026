using API.Helpers;
using Application.System.MasterData.Position.Commands;
using Application.System.MasterData.Position.Dtos;
using Application.System.MasterData.Position.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class PositionEndpoints
    {
        public static IEndpointRouteBuilder MapPositionEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/positions")
                .WithTags("Positions");

            // GET /api/hr/master-data/positions
            group.MapGet("/", async (
                IMediator mediator,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new ListPositions.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllPositions");

            // GET /api/hr/master-data/positions/paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedPositions.Query(pageNumber, pageSize, searchTerm),
                    ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedPositions");

            // GET /api/hr/master-data/positions/{id}
            group.MapGet("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new GetPositionById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetPositionById");

            // POST /api/hr/master-data/positions
            group.MapPost("/", async (
                IMediator mediator,
                CreatePositionDto dto,
                CancellationToken ct = default) =>
            {
                var id = await mediator.Send(new CreatePosition.Command(dto), ct);
                return Results.Created($"/api/hr/master-data/positions/{id}", new { id });
            })
            .WithName("CreatePosition");

            // PUT /api/hr/master-data/positions/{id}
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdatePositionDto dto,
                CancellationToken ct = default) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdatePosition.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdatePosition");

            // DELETE /api/hr/master-data/positions/{id} (Soft Delete)
            group.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct = default) =>
            {
                await mediator.Send(new SoftDeletePosition.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeletePosition");

            return routes;
        }
    }
}