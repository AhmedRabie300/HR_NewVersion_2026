using Application.Common.Abstractions;
using Application.System.MasterData.Project.Commands;
using Application.System.MasterData.Project.Dtos;
using Application.System.MasterData.Project.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class ProjectEndpoints
    {
        public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/projects")
                .WithTags("Projects");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListProjects.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllProjects");

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedProjects.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedProjects");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetProjectById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetProjectById");

             group.MapPost("/", async (
                IMediator mediator,
                CreateProjectDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateProject.Command(dto), ct);
                return Results.Created($"/master-data/projects/{id}", new { id });
            })
            .WithName("CreateProject");

             group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateProjectDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateProject.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateProject");

             group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteProject.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteProject");

             group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteProject.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteProject");

            return routes;
        }
    }
}