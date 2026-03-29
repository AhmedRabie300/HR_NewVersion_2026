using API.Helpers;
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
            var group = routes.MapGroup("/api/hr/master-data/document-types-groups")
                .WithTags("DocumentTypesGroups");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListDocumentTypesGroups.Query(), ct);
                return Results.Ok(result);
            })
            //.RequirePermission("DocumentTypesGroups", "View")
            .WithName("GetAllDocumentTypesGroups");

            // GET paged
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
            //.RequirePermission("DocumentTypesGroups", "View")
            .WithName("GetPagedDocumentTypesGroups");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetDocumentTypesGroupById.Query(id), ct);
                return Results.Ok(result);
            })
            //.RequirePermission("DocumentTypesGroups", "View")
            .WithName("GetDocumentTypesGroupById");

            // POST create
            group.MapPost("/", async (IMediator mediator, CreateDocumentTypesGroupDto dto, CancellationToken ct = default) =>
            {
                var id = await mediator.Send(new CreateDocumentTypesGroup.Command(dto), ct);
                return Results.Created($"/api/hr/master-data/document-types-groups/{id}", new { id });
            })
            //.RequirePermission("DocumentTypesGroups", "Add")
            .WithName("CreateDocumentTypesGroup");

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateDocumentTypesGroupDto dto,
                CancellationToken ct = default) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateDocumentTypesGroup.Command(fixedDto), ct);
                return Results.NoContent();
            })
            //.RequirePermission("DocumentTypesGroups", "Edit")
            .WithName("UpdateDocumentTypesGroup");

            // DELETE hard
            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new DeleteDocumentTypesGroup.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            //.RequirePermission("DocumentTypesGroups", "Delete")
            .WithName("DeleteDocumentTypesGroup");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct = default) =>
            {
                await mediator.Send(new SoftDeleteDocumentTypesGroup.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            //.RequirePermission("DocumentTypesGroups", "Delete")
            .WithName("SoftDeleteDocumentTypesGroup");

            return routes;
        }
    }
}