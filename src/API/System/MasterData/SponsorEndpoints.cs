using Application.System.MasterData.Sponsor.Commands;
using Application.System.MasterData.Sponsor.Dtos;
using Application.System.MasterData.Sponsor.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class SponsorEndpoints
    {
        public static IEndpointRouteBuilder MapSponsorEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/sponsors")
                .WithTags("Sponsors");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListSponsors.Query(), ct);
                return Results.Ok(result);
            })
            //.RequirePermission("Sponsors", "View")
            .WithName("GetAllSponsors");

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                int? companyId = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedSponsors.Query(pageNumber, pageSize, searchTerm, companyId), ct);
                return Results.Ok(result);
            })
            //.RequirePermission("Sponsors", "View")
            .WithName("GetPagedSponsors");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetSponsorById.Query(id), ct);
                return Results.Ok(result);
            })
            //.RequirePermission("Sponsors", "View")
            .WithName("GetSponsorById");

            // POST create
            group.MapPost("/", async (IMediator mediator, CreateSponsorDto dto, CancellationToken ct = default) =>
            {
                var id = await mediator.Send(new CreateSponsor.Command(dto), ct);
                return Results.Created($"/api/hr/master-data/sponsors/{id}", new { id });
            })
            //.RequirePermission("Sponsors", "Add")
            .WithName("CreateSponsor");

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateSponsorDto dto,
                CancellationToken ct = default) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateSponsor.Command(fixedDto), ct);
                return Results.NoContent();
            })
            //.RequirePermission("Sponsors", "Edit")
            .WithName("UpdateSponsor");

            // DELETE hard
            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new DeleteSponsor.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            //.RequirePermission("Sponsors", "Delete")
            .WithName("DeleteSponsor");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct = default) =>
            {
                await mediator.Send(new SoftDeleteSponsor.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            //.RequirePermission("Sponsors", "Delete")
            .WithName("SoftDeleteSponsor");

            return routes;
        }
    }
}