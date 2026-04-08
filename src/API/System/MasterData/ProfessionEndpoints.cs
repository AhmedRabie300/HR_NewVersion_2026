// API/System/MasterData/ProfessionEndpoints.cs
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
            var group = routes.MapGroup("/master-data/professions")
                .WithTags("Professions");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListProfessions.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllProfessions");

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedProfessions.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedProfessions");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetProfessionById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetProfessionById");

            // POST create
            group.MapPost("/", async (IMediator mediator, CreateProfessionDto dto, CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateProfession.Command(dto), ct);
                return Results.Created($"/master-data/professions/{id}", new { id });
            })
            .WithName("CreateProfession");

            // PUT update
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
            .WithName("UpdateProfession");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteProfession.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteProfession");

            return routes;
        }
    }
}