using Application.Common.Abstractions;
using Application.System.MasterData.Position.Commands;
using Application.System.MasterData.Position.Dtos;
using Application.System.MasterData.Position.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class PositionEndpoints
    {
        public static IEndpointRouteBuilder MapPositionEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/positions")
                .WithTags("Positions");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListPositions.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllPositions");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedPositions.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedPositions");
 
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetPositionById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetPositionById");

            group.MapPost("/", async (
                IMediator mediator,
                [FromHeader(Name = "CompanyId")] int companyId,
                [FromServices] IContextService contextService,
                CreatePositionDto dto,
                CancellationToken ct) =>
            {
                var regUserId = contextService.GetCurrentUserId();
                var id = await mediator.Send(new CreatePosition.Command(companyId, regUserId, dto), ct);
                return Results.Created($"/master-data/positions/{id}", new { id });
            })
            .WithName("CreatePosition");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdatePositionDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdatePosition.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdatePosition");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeletePosition.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeletePosition");

          
            return routes;
        }
    }
}