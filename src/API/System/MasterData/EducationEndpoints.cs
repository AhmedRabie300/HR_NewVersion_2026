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
            var group = routes.MapGroup("/api/hr/master-data/educations")
                .WithTags("Educations");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListEducations.Query(), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Educations", "View")
            .WithName("GetAllEducations");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                int? companyId = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedEducations.Query(pageNumber, pageSize, searchTerm, companyId), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Educations", "View")
            .WithName("GetPagedEducations");

          
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetEducationById.Query(id), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Educations", "View")
            .WithName("GetEducationById");

            group.MapPost("/", async (IMediator mediator, CreateEducationDto dto, CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateEducation.Command(dto), ct);
                return Results.Created($"/api/hr/master-data/educations/{id}", new { id });
            })
           // .RequirePermission("Educations", "Add")
            .WithName("CreateEducation");

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
           // .RequirePermission("Educations", "Edit")
            .WithName("UpdateEducation");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteEducation.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
           // .RequirePermission("Educations", "Delete")
            .WithName("DeleteEducation");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteEducation.Command(id, regUserId), ct);
                return Results.NoContent();
            })
           // .RequirePermission("Educations", "Delete")
            .WithName("SoftDeleteEducation");

            return routes;
        }
    }
}