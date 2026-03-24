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
            var group = routes.MapGroup("/api/hr/master-data/positions")
                .WithTags("Positions");

            // GET /api/hr/master-data/positions?lang=1
            group.MapGet("/", async (
                IMediator mediator,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new ListPositions.Query(lang), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Positions", "View")
            .WithName("GetAllPositions")
            .WithOpenApi();

            // GET /api/hr/master-data/positions/paged?pageNumber=1&pageSize=20&searchTerm=&lang=1
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedPositions.Query(pageNumber, pageSize, searchTerm, lang),
                    ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Positions", "View")
            .WithName("GetPagedPositions")
            .WithOpenApi();

            // GET /api/hr/master-data/positions/{id}?lang=1
            group.MapGet("/{id:int}", async (
                IMediator mediator,
                int id,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new GetPositionById.Query(id, lang), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Positions", "View")
            .WithName("GetPositionById")
            .WithOpenApi();

        

            // POST /api/hr/master-data/positions?lang=1
            group.MapPost("/", async (
                IMediator mediator,
                CreatePositionDto dto,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var id = await mediator.Send(new CreatePosition.Command(dto, lang), ct);
                return Results.Created($"/api/hr/master-data/positions/{id}", new { id });
            })
           // .RequirePermission("Positions", "Add")
            .WithName("CreatePosition")
            .WithOpenApi();

            // PUT /api/hr/master-data/positions/{id}?lang=1
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdatePositionDto dto,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdatePosition.Command(fixedDto, lang), ct);
                return Results.NoContent();
            })
           // .RequirePermission("Positions", "Edit")
            .WithName("UpdatePosition")
            .WithOpenApi();

            // DELETE /api/hr/master-data/positions/{id}?regUserId=&lang=1 (Soft Delete)
            group.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                int? regUserId,
                int lang = 1,
                CancellationToken ct = default) =>
            {
                await mediator.Send(new SoftDeletePosition.Command(id, regUserId, lang), ct);
                return Results.NoContent();
            })
           // .RequirePermission("Positions", "Delete")
            .WithName("SoftDeletePosition")
            .WithOpenApi();

            return routes;
        }
    }
}