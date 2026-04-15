using Application.Common.Abstractions;
using Application.System.MasterData.Sponsor.Commands;
using Application.System.MasterData.Sponsor.Dtos;
using Application.System.MasterData.Sponsor.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class SponsorEndpoints
    {
        public static IEndpointRouteBuilder MapSponsorEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/sponsors")
                .WithTags("Sponsors");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListSponsors.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllSponsors");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedSponsors.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedSponsors");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetSponsorById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetSponsorById");

            group.MapPost("/", async (
                IMediator mediator,
                [FromHeader(Name = "CompanyId")] int companyId,
                [FromServices] IContextService contextService,
                CreateSponsorDto dto,
                CancellationToken ct) =>
            {
                var regUserId = contextService.GetCurrentUserId();
                var id = await mediator.Send(new CreateSponsor.Command(companyId, regUserId, dto), ct);
                return Results.Created($"/master-data/sponsors/{id}", new { id });
            })
            .WithName("CreateSponsor");

             group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                [FromHeader(Name = "CompanyId")] int companyId,  
                UpdateSponsorDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateSponsor.Command(companyId, fixedDto), ct);  
                return Results.NoContent();
            })
            .WithName("UpdateSponsor");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteSponsor.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteSponsor");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteSponsor.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteSponsor");

            return routes;
        }
    }
}