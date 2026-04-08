// API/System/MasterData/EducationEndpoints.cs
using API.Helpers;
using Application.System.MasterData.Education.Commands;
using Application.System.MasterData.Education.Dtos;
using Application.System.MasterData.Education.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class EducationEndpoints
    {
        public static IEndpointRouteBuilder MapEducationEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/educations")
                .WithTags("Educations");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListEducations.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllEducations");

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedEducations.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedEducations");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetEducationById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetEducationById");

            // POST create
            group.MapPost("/", async (IMediator mediator, CreateEducationDto dto, CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateEducation.Command(dto), ct);
                return Results.Created($"/master-data/educations/{id}", new { id });
            })
            .WithName("CreateEducation");

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateEducationDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateEducation.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateEducation");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteEducation.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteEducation");

            return routes;
        }
    }
}