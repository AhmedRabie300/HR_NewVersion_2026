using Application.Common.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Commands;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using Application.System.MasterData.DocumentTypesGroup.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class DocumentTypesGroupEndpoints
    {
        public static IEndpointRouteBuilder MapDocumentTypesGroupEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/document-types-groups")
                .WithTags("Document Types Groups");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListDocumentTypesGroups.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllDocumentTypesGroups");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedDocumentTypesGroups.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedDocumentTypesGroups");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetDocumentTypesGroupById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetDocumentTypesGroupById");

            group.MapPost("/", async (
                IMediator mediator,
                CreateDocumentTypesGroupDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateDocumentTypesGroup.Command(dto), ct);
                return Results.Created($"/master-data/document-types-groups/{id}", new { id });
            })
            .WithName("CreateDocumentTypesGroup");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateDocumentTypesGroupDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateDocumentTypesGroup.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateDocumentTypesGroup");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteDocumentTypesGroup.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteDocumentTypesGroup");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
               await mediator.Send(new DeleteDocumentTypesGroup.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("DeleteDocumentTypesGroup");

            return routes;
        }
    }
}