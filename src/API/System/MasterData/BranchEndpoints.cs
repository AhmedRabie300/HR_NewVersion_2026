using Application.Common.Abstractions;
using Application.System.MasterData.Branch.Commands;
using Application.System.MasterData.Branch.Dtos;
using Application.System.MasterData.Branch.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class BranchEndpoints
    {
        public static IEndpointRouteBuilder MapBranchEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/branches")
                .WithTags("Branches");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListBranches.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllBranches");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedBranches.Query(pageNumber, pageSize, searchTerm),
                    ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedBranches");

            group.MapGet("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetBranchById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetBranchById");

            // ✅ GET by company - باستخدام CompanyId من Header
            group.MapGet("/by-company", async (
                IMediator mediator,
                [FromServices] IContextService contextService,
                CancellationToken ct) =>
            {
                var companyId = contextService.GetCurrentCompanyId();
                var result = await mediator.Send(new GetBranchesByCompany.Query(companyId), ct);
                return Results.Ok(result);
            })
            .WithName("GetBranchesByCompany");

            // ✅ POST create - باستخدام CompanyId من Header
            group.MapPost("/", async (
                IMediator mediator,
                [FromHeader(Name = "CompanyId")] int companyId,
                [FromServices] IContextService contextService,
                CreateBranchDto dto,
                CancellationToken ct) =>
            {
                var regUserId = contextService.GetCurrentUserId();
                var id = await mediator.Send(new CreateBranch.Command(companyId, regUserId, dto), ct);
                return Results.Created($"/master-data/branches/{id}", new { id });
            })
            .WithName("CreateBranch");

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateBranchDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateBranch.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateBranch");

            // ✅ DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteBranch.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteBranch");

            // ✅ DELETE hard
            group.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new DeleteBranch.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteBranch");

            return routes;
        }
    }
}