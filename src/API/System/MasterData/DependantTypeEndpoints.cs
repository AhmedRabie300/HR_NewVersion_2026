using Application.Common.Abstractions;
using Application.System.MasterData.DependantType.Commands;
using Application.System.MasterData.DependantType.Dtos;
using Application.System.MasterData.DependantType.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class DependantTypeEndpoints
    {
        public static IEndpointRouteBuilder MapDependantTypeEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/dependant-types")
                .WithTags("DependantTypes");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListDependantTypes.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllDependantTypes");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedDependantTypes.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedDependantTypes");

         
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetDependantTypeById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetDependantTypeById");

            group.MapPost("/", async (
                IMediator mediator,
                [FromHeader(Name = "CompanyId")] int companyId,
                [FromServices] IContextService contextService,
                CreateDependantTypeDto dto,
                CancellationToken ct) =>
            {
                var regUserId = contextService.GetCurrentUserId();
                var id = await mediator.Send(new CreateDependantType.Command(companyId, regUserId, dto), ct);
                return Results.Created($"/master-data/dependant-types/{id}", new { id });
            })
            .WithName("CreateDependantType");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateDependantTypeDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateDependantType.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateDependantType");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteDependantType.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteDependantType");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteDependantType.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteDependantType");

            return routes;
        }
    }
}