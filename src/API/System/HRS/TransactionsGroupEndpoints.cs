using Application.Common.Abstractions;
using Application.System.HRS.TransactionsGroup.Commands;
using Application.System.HRS.TransactionsGroup.Dtos;
using Application.System.HRS.TransactionsGroup.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS
{
    public static class TransactionsGroupEndpoints
    {
        public static IEndpointRouteBuilder MapTransactionsGroupEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/hrs/transactions-groups")
                .WithTags("Transactions Groups");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListTransactionsGroups.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllTransactionsGroups");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedTransactionsGroups.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedTransactionsGroups");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetTransactionsGroupById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetTransactionsGroupById");
 
            group.MapPost("/", async (
                IMediator mediator,
                [FromHeader(Name = "CompanyId")] int companyId,
                [FromServices] IContextService contextService,
                CreateTransactionsGroupDto dto,
                CancellationToken ct) =>
            {
                var regUserId = contextService.GetCurrentUserId();
                var id = await mediator.Send(new CreateTransactionsGroup.Command(companyId, regUserId, dto), ct);
                return Results.Created($"/hrs/transactions-groups/{id}", new { id });
            })
            .WithName("CreateTransactionsGroup");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateTransactionsGroupDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateTransactionsGroup.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateTransactionsGroup");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteTransactionsGroup.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteTransactionsGroup");

            group.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteTransactionsGroup.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteTransactionsGroup");

            return routes;
        }
    }
}