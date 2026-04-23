using Application.Common.Abstractions;
using Application.System.HRS.Basics.HICompanies.Commands;
using Application.System.HRS.Basics.HICompanies.Dtos;
using Application.System.HRS.Basics.HICompanies.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS
{
    public static class HICompanyEndpoints
    {
        public static IEndpointRouteBuilder MapHICompanyEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/basics/hi-companies")
                .WithTags("HICompanies");

            // ==================== HICompany (Master) ====================

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListHICompanies.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllHICompanies")
            .WithOpenApi();

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedHICompanies.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedHICompanies")
            .WithOpenApi();

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetHICompanyById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetHICompanyById")
            .WithOpenApi();

            // GET classes by hiCompanyId
            group.MapGet("/{hiCompanyId:int}/classes", async (IMediator mediator, int hiCompanyId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetHICompanyClasses.Query(hiCompanyId), ct);
                return Results.Ok(result);
            })
            .WithName("GetHICompanyClasses")
            .WithOpenApi();

            // POST create
            group.MapPost("/", async (
                IMediator mediator,
                CreateHICompanyDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateHICompany.Command(dto), ct);
                return Results.Created($"/basics/hi-companies/{id}", new { id });
            })
            .WithName("CreateHICompany")
            .WithOpenApi();

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateHICompanyDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateHICompany.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateHICompany")
            .WithOpenApi();

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                [FromQuery] int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteHICompany.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteHICompany")
            .WithOpenApi();

            // DELETE hard
            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteHICompany.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteHICompany")
            .WithOpenApi();

            // ==================== HICompanyClass (Detail) ====================

            // POST add class to hiCompany
            group.MapPost("/{hiCompanyId:int}/classes", async (
                IMediator mediator,
                int hiCompanyId,
                CreateHICompanyClassDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new AddHICompanyClass.Command(hiCompanyId, dto), ct);
                return Results.Created($"/basics/hi-companies/{hiCompanyId}/classes/{id}", new { id });
            })
            .WithName("AddHICompanyClass")
            .WithOpenApi();

            // PUT update class
            group.MapPut("/classes/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateHICompanyClassDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateHICompanyClass.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateHICompanyClass")
            .WithOpenApi();

            // DELETE soft class
            group.MapDelete("/classes/{id:int}/soft", async (
                IMediator mediator,
                int id,
                [FromQuery] int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteHICompanyClass.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteHICompanyClass")
            .WithOpenApi();

            // DELETE hard class
            group.MapDelete("/classes/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteHICompanyClass.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteHICompanyClass")
            .WithOpenApi();

            return routes;
        }
    }
}