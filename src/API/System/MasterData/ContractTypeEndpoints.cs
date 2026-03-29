using API.Helpers;
using Application.System.MasterData.ContractType.Commands;
using Application.System.MasterData.ContractType.Dtos;
using Application.System.MasterData.ContractType.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.MasterData
{
    public static class ContractTypeEndpoints
    {
        public static IEndpointRouteBuilder MapContractTypeEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/hr/master-data/contract-types")
                .WithTags("ContractTypes");

            // GET all
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListContractTypes.Query(), ct);
                return Results.Ok(result);
            })
            //.RequirePermission("ContractTypes", "View")
            .WithName("GetAllContractTypes");

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
                    new GetPagedContractTypes.Query(pageNumber, pageSize, searchTerm, companyId), ct);
                return Results.Ok(result);
            })
            //.RequirePermission("ContractTypes", "View")
            .WithName("GetPagedContractTypes");

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetContractTypeById.Query(id), ct);
                return Results.Ok(result);
            })
            //.RequirePermission("ContractTypes", "View")
            .WithName("GetContractTypeById");

            // POST create
            group.MapPost("/", async (IMediator mediator, CreateContractTypeDto dto, CancellationToken ct = default) =>
            {
                var id = await mediator.Send(new CreateContractType.Command(dto), ct);
                return Results.Created($"/api/hr/master-data/contract-types/{id}", new { id });
            })
            //.RequirePermission("ContractTypes", "Add")
            .WithName("CreateContractType");

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateContractTypeDto dto,
                CancellationToken ct = default) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateContractType.Command(fixedDto), ct);
                return Results.NoContent();
            })
            //.RequirePermission("ContractTypes", "Edit")
            .WithName("UpdateContractType");

            // DELETE hard
            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new DeleteContractType.Command(id), ct);
                return result ? Results.NoContent() : Results.NotFound();
            })
            //.RequirePermission("ContractTypes", "Delete")
            .WithName("DeleteContractType");

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct = default) =>
            {
                await mediator.Send(new SoftDeleteContractType.Command(id, regUserId), ct);
                return Results.NoContent();
            })
            //.RequirePermission("ContractTypes", "Delete")
            .WithName("SoftDeleteContractType");

            return routes;
        }
    }
}