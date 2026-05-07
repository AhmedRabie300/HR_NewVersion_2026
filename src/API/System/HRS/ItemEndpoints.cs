using Application.Common.Abstractions;
using Application.System.HRS.Basics.Items.Commands;
using Application.System.HRS.Basics.Items.Dtos;
using Application.System.HRS.Basics.Items.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS
{
    public static class ItemEndpoints
    {
        public static IEndpointRouteBuilder MapItemEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/basics/items")
                .WithTags("Items");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListItems.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllItems")
            ;

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedItems.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedItems")
            ;

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetItemById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetItemById")
            ;

            // POST create
            group.MapPost("/", async (
                IMediator mediator,
                CreateItemDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateItem.Command(dto), ct);
                return Results.Created($"/basics/items/{id}", new { id });
            })
            .WithName("CreateItem")
            ;

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateItemDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateItem.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateItem")
            ;

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                 CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteItem.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteItem")
            ;

            // DELETE hard
            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteItem.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteItem")
            ;

            return routes;
        }
    }
}