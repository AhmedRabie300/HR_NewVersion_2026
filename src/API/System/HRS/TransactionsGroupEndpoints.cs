using Application.System.HRS.TransactionsGroup.Commands;
using Application.System.HRS.TransactionsGroup.Dtos;
using Application.System.HRS.TransactionsGroup.Queries;
using MediatR;

namespace API.System.HRS
{
    public static class TransactionsGroupEndpoints
    {
        public static IEndpointRouteBuilder MapTransactionsGroupEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/hrs/transactions-groups")
                .WithTags("Transactions Groups");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListTransactionsGroups.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllTransactionsGroups");

            // GET paged
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

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetTransactionsGroupById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetTransactionsGroupById");

            // GET by company id
            group.MapGet("/by-company/{companyId:int}", async (IMediator mediator, int companyId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetTransactionsGroupsByCompanyId.Query(companyId), ct);
                return Results.Ok(result);
            })
            .WithName("GetTransactionsGroupsByCompanyId");

            // POST create
            group.MapPost("/", async (IMediator mediator, CreateTransactionsGroupDto dto, CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateTransactionsGroup.Command(dto), ct);
                return Results.Created($"/hrs/transactions-groups/{id}", new { id });
            })
            .WithName("CreateTransactionsGroup");

            // PUT update
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

            // DELETE soft
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

            // DELETE hard
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