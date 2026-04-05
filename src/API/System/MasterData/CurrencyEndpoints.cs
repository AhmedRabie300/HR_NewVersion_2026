using API.Helpers;
using Application.System.MasterData.Currency.Commands;
using Application.System.MasterData.Currency.Dtos;
using Application.System.MasterData.Currency.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class CurrencyEndpoints
    {
        public static IEndpointRouteBuilder MapCurrencyEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/currencies")
                .WithTags("Currencies");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListCurrencies.Query(), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Currencies", "View")
            .WithName("GetAllCurrencies");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                int? companyId = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedCurrencies.Query(pageNumber, pageSize, searchTerm, companyId), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Currencies", "View")
            .WithName("GetPagedCurrencies");

      

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetCurrencyById.Query(id), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Currencies", "View")
            .WithName("GetCurrencyById");

            group.MapPost("/", async (IMediator mediator, CreateCurrencyDto dto, CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateCurrency.Command(dto), ct);
                return Results.Created($"/api/hr/master-data/currencies/{id}", new { id });
            })
           // .RequirePermission("Currencies", "Add")
            .WithName("CreateCurrency");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateCurrencyDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateCurrency.Command(fixedDto), ct);
                return Results.NoContent();
            })
           // .RequirePermission("Currencies", "Edit")
            .WithName("UpdateCurrency");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteCurrency.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
           // .RequirePermission("Currencies", "Delete")
            .WithName("DeleteCurrency");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteCurrency.Command(id, regUserId), ct);
                return Results.NoContent();
            })
           // .RequirePermission("Currencies", "Delete")
            .WithName("SoftDeleteCurrency");

            return routes;
        }
    }
}