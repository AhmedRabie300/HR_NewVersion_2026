using Application.Common.Abstractions;
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
            var group = routes.MapGroup("/master-data/contract-types")
                .WithTags("ContractTypes");

            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListContractTypes.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllContractTypes");

            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedContractTypes.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedContractTypes");

         
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetContractTypeById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetContractTypeById");

            group.MapPost("/", async (
                IMediator mediator,
                CreateContractTypeDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateContractType.Command( dto), ct);
                return Results.Created($"/master-data/contract-types/{id}", new { id });
            })
            .WithName("CreateContractType");

            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateContractTypeDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateContractType.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateContractType");

            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                int? regUserId,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteContractType.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteContractType");

            group.MapDelete("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
               await mediator.Send(new DeleteContractType.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("DeleteContractType");

            return routes;
        }
    }
}