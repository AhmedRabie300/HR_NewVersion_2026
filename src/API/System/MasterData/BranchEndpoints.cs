using Application.System.MasterData.Branch.Commands;
using Application.System.MasterData.Branch.Dtos;
using Application.System.MasterData.Branch.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class BranchEndpoints
    {
        public static IEndpointRouteBuilder MapBranchEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/master-data/branches")
                .WithTags("Branches");

            // GET /api/hr/master-data/branches
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

            // GET /api/hr/master-data/branches/by-company/{companyId}
            group.MapGet("/by-company/{companyId:int}", async (
                IMediator mediator,
                int companyId,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetBranchesByCompany.Query(companyId), ct);
                return Results.Ok(result);
            })
            .WithName("GetBranchesByCompany");

            // POST /api/hr/master-data/branches
            group.MapPost("/", async (
                IMediator mediator,
                CreateBranchDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateBranch.Command(dto), ct);
                return Results.Created($"/master-data/branches/{id}", new { id });
            })
            .WithName("CreateBranch");

            // PUT /api/hr/master-data/branches/{id}
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

           

            return routes;
        }
    }
}