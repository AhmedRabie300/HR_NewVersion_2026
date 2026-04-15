using Application.Common.Abstractions;
using Application.System.MasterData.BloodGroup.Commands;
using Application.System.MasterData.BloodGroup.Dtos;
using Application.System.MasterData.BloodGroup.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class BloodGroupEndpoints
    {
        public static IEndpointRouteBuilder MapBloodGroupEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/blood-groups")
                .WithTags("BloodGroups");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListBloodGroups.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllBloodGroups");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedBloodGroups.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedBloodGroups");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetBloodGroupById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetBloodGroupById");

            group.MapPost("/", async (
                IMediator mediator,
                [FromHeader(Name = "CompanyId")] int companyId,
                [FromServices] IContextService contextService,
                CreateBloodGroupDto dto,
                CancellationToken ct) =>
            {
                var regUserId = contextService.GetCurrentUserId();
                var id = await mediator.Send(new CreateBloodGroup.Command(companyId, regUserId, dto), ct);
                return Results.Created($"/master-data/blood-groups/{id}", new { id });
            })
            .WithName("CreateBloodGroup");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateBloodGroupDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateBloodGroup.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateBloodGroup");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteBloodGroup.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteBloodGroup");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteBloodGroup.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteBloodGroup");

            return routes;
        }
    }
}