using Application.Common.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.TransactionType.Commands;
using Application.System.HRS.Basics.FiscalTransactions.TransactionType.Dtos;
using Application.System.HRS.Basics.FiscalTransactions.TransactionType.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS
{
    public static class TransactionsTypeEndpoints
    {
        public static IEndpointRouteBuilder MapTransactionsTypeEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/basics/fiscal-transactions/transactions-types")
                .WithTags("TransactionsTypes");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListTransactionsTypes.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllTransactionsTypes");

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedTransactionsTypes.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedTransactionsTypes");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetTransactionsTypeById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetTransactionsTypeById");

            // POST create
            group.MapPost("/", async (
                IMediator mediator,
                CreateTransactionsTypeDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateTransactionsType.Command(dto), ct);
                return Results.Created($"/basics/fiscal-transactions/transactions-types/{id}", new { id });
            })
            .WithName("CreateTransactionsType");

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateTransactionsTypeDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateTransactionsType.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateTransactionsType");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteTransactionsType.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteTransactionsType");

            // DELETE hard
            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteTransactionsType.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteTransactionsType");

            return routes;
        }
    }
}