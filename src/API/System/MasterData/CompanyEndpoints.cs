using API.Helpers;
using Application.System.MasterData.Company.Commands;
using Application.System.MasterData.Company.Dtos;
using Application.System.MasterData.Company.Queries;
using MediatR;

namespace API.System.MasterData
{
    public static class CompanyEndpoints
    {
        public static IEndpointRouteBuilder MapCompanyEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/hr/master-data/companies")
                .WithTags("Companies");

            // GET /api/hr/master-data/companies
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListCompanies.Query(), ct);
                return Results.Ok(result);
            })
           //// .RequirePermission("Companies", "View")
            .WithName("GetAllCompanies");

            // GET /api/hr/master-data/companies/paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedCompanies.Query(pageNumber, pageSize, searchTerm),
                    ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Companies", "View")
            .WithName("GetPagedCompanies");

            // GET /api/hr/master-data/companies/{id}
            group.MapGet("/{id:int}", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetCompanyById.Query(id), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Companies", "View")
            .WithName("GetCompanyById");

            // GET /api/hr/master-data/companies/by-code/{code}
            group.MapGet("/by-code/{code}", async (
                IMediator mediator,
                string code,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetCompanyByCode.Query(code), ct);
                return Results.Ok(result);
            })
           // .RequirePermission("Companies", "View")
            .WithName("GetCompanyByCode");

            // POST /api/hr/master-data/companies
            group.MapPost("/", async (
                IMediator mediator,
                CreateCompanyDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateCompany.Command(dto), ct);
                return Results.Created($"/api/hr/master-data/companies/{id}", new { id });
            })
           // .RequirePermission("Companies", "Add")
            .WithName("CreateCompany");

            // PUT /api/hr/master-data/companies/{id}
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateCompanyDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateCompany.Command(fixedDto), ct);
                return Results.NoContent();
            })
           // .RequirePermission("Companies", "Edit")
            .WithName("UpdateCompany");

            // DELETE /api/hr/master-data/companies/{id} (Soft Delete)
            group.MapDelete("/{id:int}", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                // هنحتاج نضيف Soft Delete Command
                // await mediator.Send(new SoftDeleteCompany.Command(id, regUserId), ct);
                return Results.NoContent();
            })
           // .RequirePermission("Companies", "Delete")
            .WithName("SoftDeleteCompany");

            return routes;
        }
    }
}