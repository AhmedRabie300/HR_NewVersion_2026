using Application.Common.Abstractions;
using Application.System.HRS.Gender.Commands;
using Application.System.HRS.Gender.Dtos;
using Application.System.HRS.Gender.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS
{
    public static class GenderEndpoints
    {
        public static IEndpointRouteBuilder MapGenderEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/hrs/genders")
                .WithTags("Genders");

          
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedGenders.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedGenders");

            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetGenderById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetGenderById");

            group.MapPost("/", async (
                IMediator mediator,
                [FromHeader(Name = "CompanyId")] int companyId,
                [FromServices] IContextService contextService,
                CreateGenderDto dto,
                CancellationToken ct) =>
            {
                var regUserId = contextService.GetCurrentUserId();
                var id = await mediator.Send(new CreateGender.Command(companyId, regUserId, dto), ct);
                return Results.Created($"/hrs/genders/{id}", new { id });
            })
            .WithName("CreateGender");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateGenderDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateGender.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateGender");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteGender.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteGender");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteGender.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteGender");

            return routes;
        }
    }
}