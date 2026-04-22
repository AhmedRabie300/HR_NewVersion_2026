using Application.Common.Abstractions;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Commands;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS.VacationAndEndOfService
{
    public static class EndOfServiceEndpoints
    {
        public static IEndpointRouteBuilder MapEndOfServiceEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/basics/vacation-end-of-service/end-of-services")
                .WithTags("EndOfServices");

            // ==================== EndOfService (Header) ====================

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListEndOfServices.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllEndOfServices");

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedEndOfServices.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedEndOfServices");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetEndOfServiceById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetEndOfServiceById");

            // GET rules by endOfServiceId
            group.MapGet("/{endOfServiceId:int}/rules", async (IMediator mediator, int endOfServiceId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetEndOfServiceRules.Query(endOfServiceId), ct);
                return Results.Ok(result);
            })
            .WithName("GetEndOfServiceRules");

            // POST create
            group.MapPost("/", async (
                IMediator mediator,
                CreateEndOfServiceDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateEndOfService.Command(dto), ct);
                return Results.Created($"/basics/vacation-end-of-service/end-of-services/{id}", new { id });
            })
            .WithName("CreateEndOfService");

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateEndOfServiceDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateEndOfService.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateEndOfService");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteEndOfService.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteEndOfService");

            // DELETE hard
            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteEndOfService.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteEndOfService");

            // ==================== EndOfServiceRule (Detail) ====================

            // POST add rule to endOfService
            group.MapPost("/{endOfServiceId:int}/rules", async (
                IMediator mediator,
                int endOfServiceId,
                CreateEndOfServiceRuleDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new AddEndOfServiceRule.Command(endOfServiceId, dto), ct);
                return Results.Created($"/basics/vacation-end-of-service/end-of-services/{endOfServiceId}/rules/{id}", new { id });
            })
            .WithName("AddEndOfServiceRule");

            // PUT update rule
            group.MapPut("/rules/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateEndOfServiceRuleDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateEndOfServiceRule.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateEndOfServiceRule");

            // DELETE soft rule
            group.MapDelete("/rules/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteEndOfServiceRule.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteEndOfServiceRule");

            // DELETE hard rule
            group.MapDelete("/rules/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteEndOfServiceRule.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteEndOfServiceRule");

            return routes;
        }
    }
}