using Application.System.MasterData.Bank.Commands;
using Application.System.MasterData.Bank.Dtos;
using Application.System.MasterData.Bank.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class BankEndpoints
    {
        public static IEndpointRouteBuilder MapBankEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/banks")
                .WithTags("Banks");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListBanks.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllBanks");

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedBanks.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedBanks");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetBankById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetBankById");

            // GET by company id
            group.MapGet("/by-company/{companyId:int}", async (IMediator mediator, int companyId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetBanksByCompanyId.Query(companyId), ct);
                return Results.Ok(result);
            })
            .WithName("GetBanksByCompanyId");

            // POST create
            group.MapPost("/", async (IMediator mediator, CreateBankDto dto, CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateBank.Command(dto), ct);
                return Results.Created($"/master-data/banks/{id}", new { id });
            })
            .WithName("CreateBank");

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateBankDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateBank.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateBank");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteBank.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteBank");

            // DELETE hard
            group.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteBank.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteBank");

            return routes;
        }
    }
}