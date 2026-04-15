using Application.Common.Abstractions;
using Application.System.MasterData.Education.Commands;
using Application.System.MasterData.Education.Dtos;
using Application.System.MasterData.Education.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class EducationEndpoints
    {
        public static IEndpointRouteBuilder MapEducationEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/educations")
                .WithTags("Educations");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListEducations.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllEducations");

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
 
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetEducationById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetEducationById");

            group.MapPost("/", async (
                IMediator mediator,
                [FromHeader(Name = "CompanyId")] int companyId,
                [FromServices] IContextService contextService,
                CreateEducationDto dto,
                CancellationToken ct) =>
            {
                var regUserId = contextService.GetCurrentUserId();
                var id = await mediator.Send(new CreateEducation.Command(companyId, regUserId, dto), ct);
                return Results.Created($"/master-data/educations/{id}", new { id });
            })
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
            .WithName("UpdateEducation");

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

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteEducation.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteEducation");

            return routes;
        }
    }
}