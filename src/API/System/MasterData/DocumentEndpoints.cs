using API.Helpers;
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
            var group = routes.MapGroup("/api/hr/master-data/documents")
                .WithTags("Documents");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListDocuments.Query(), ct);
                return Results.Ok(result);
            })
            //.RequirePermission("Documents", "View")
            .WithName("GetAllDocuments");

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                int? groupId = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedDocuments.Query(pageNumber, pageSize, searchTerm, groupId), ct);
                return Results.Ok(result);
            })
            //.RequirePermission("Documents", "View")
            .WithName("GetPagedDocuments");

            // GET by group
            group.MapGet("/by-group/{groupId:int}", async (IMediator mediator, int groupId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetDocumentsByGroup.Query(groupId), ct);
                return Results.Ok(result);
            })
            //.RequirePermission("Documents", "View")
            .WithName("GetDocumentsByGroup");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetDocumentById.Query(id), ct);
                return Results.Ok(result);
            })
            //.RequirePermission("Documents", "View")
            .WithName("GetDocumentById");

            // POST create
            group.MapPost("/", async (IMediator mediator, CreateDocumentDto dto, CancellationToken ct = default) =>
            {
                var id = await mediator.Send(new CreateDocument.Command(dto), ct);
                return Results.Created($"/api/hr/master-data/documents/{id}", new { id });
            })
            //.RequirePermission("Documents", "Add")
            .WithName("CreateDocument");

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateDocumentDto dto,
                CancellationToken ct = default) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateDocument.Command(fixedDto), ct);
                return Results.NoContent();
            })
            //.RequirePermission("Documents", "Edit")
            .WithName("UpdateDocument");

            // DELETE hard
            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new DeleteDocument.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            //.RequirePermission("Documents", "Delete")
            .WithName("DeleteDocument");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct = default) =>
            {
                await mediator.Send(new SoftDeleteDocument.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            //.RequirePermission("Documents", "Delete")
            .WithName("SoftDeleteDocument");

            return routes;
        }
    }
}