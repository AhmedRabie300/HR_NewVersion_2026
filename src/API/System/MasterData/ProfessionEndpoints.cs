using API.Helpers;
using Application.System.MasterData.Profession.Commands;
using Application.System.MasterData.Profession.Dtos;
using Application.System.MasterData.Profession.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class ProfessionEndpoints
    {
        public static IEndpointRouteBuilder MapProfessionEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/hr/master-data/professions")
                .WithTags("Professions");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListProfessions.Query(), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Professions", "View")
            .WithName("GetAllProfessions");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                int? companyId = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedProfessions.Query(pageNumber, pageSize, searchTerm, companyId), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Professions", "View")
            .WithName("GetPagedProfessions");
 
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetProfessionById.Query(id), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Professions", "View")
            .WithName("GetProfessionById");

            group.MapPost("/", async (IMediator mediator, CreateProfessionDto dto, CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateProfession.Command(dto), ct);
                return Results.Created($"/api/hr/master-data/professions/{id}", new { id });
            })
           // .RequirePermission("Professions", "Add")
            .WithName("CreateProfession");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateProfessionDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateProfession.Command(fixedDto), ct);
                return Results.NoContent();
            })
           // .RequirePermission("Professions", "Edit")
            .WithName("UpdateProfession");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteProfession.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
           // .RequirePermission("Professions", "Delete")
            .WithName("DeleteProfession");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteProfession.Command(id, regUserId), ct);
                return Results.NoContent();
            })
           // .RequirePermission("Professions", "Delete")
            .WithName("SoftDeleteProfession");

            return routes;
        }
    }
}