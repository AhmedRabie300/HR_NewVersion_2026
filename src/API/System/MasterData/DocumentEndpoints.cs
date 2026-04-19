using Application.Common.Abstractions;
using Application.System.MasterData.Document.Commands;
using Application.System.MasterData.Document.Dtos;
using Application.System.MasterData.Document.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class DocumentEndpoints
    {
        public static IEndpointRouteBuilder MapDocumentEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/documents")
                .WithTags("Documents");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListDocuments.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllDocuments");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedDocuments.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedDocuments");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetDocumentById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetDocumentById");

            group.MapPost("/", async (
                IMediator mediator,
                CreateDocumentDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateDocument.Command( dto), ct);
                return Results.Created($"/master-data/documents/{id}", new { id });
            })
            .WithName("CreateDocument");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateDocumentDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateDocument.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateDocument");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteDocument.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteDocument");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
               await mediator.Send(new DeleteDocument.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("DeleteDocument");

            return routes;
        }
    }
}