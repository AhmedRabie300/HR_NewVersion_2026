using Application.Common.Abstractions;
using Application.System.HRS.Contracts.Commands;
using Application.System.HRS.Contracts.Dtos;
using Application.System.HRS.Contracts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.HRS
{
    public static class ContractEndpoints
    {
        public static IEndpointRouteBuilder MapContractEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/basics/contracts")
                .WithTags("Contracts");

            // ==================== Contract (Master) ====================

            // GET all (summary list)
            group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new ListContracts.Query(), ct);
                return Results.Ok(result);
            })
            .WithName("GetAllContracts");
             

            // GET paged
            group.MapGet("/paged", async (
                IMediator mediator,
                int pageNumber = 1,
                int pageSize = 20,
                string? searchTerm = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(
                    new GetPagedContracts.Query(pageNumber, pageSize, searchTerm), ct);
                return Results.Ok(result);
            })
            .WithName("GetPagedContracts");
             

            // GET by id
            group.MapGet("/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetContractById.Query(id), ct);
                return Results.Ok(result);
            })
            .WithName("GetContractById");
             

            // GET by number
            group.MapGet("/by-number/{number:int}", async (IMediator mediator, int number, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetContractByNumber.Query(number), ct);
                return Results.Ok(result);
            })
            .WithName("GetContractByNumber");
             

            // GET by employee id
            group.MapGet("/by-employee/{employeeId:int}", async (IMediator mediator, int employeeId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetContractsByEmployeeId.Query(employeeId), ct);
                return Results.Ok(result);
            })
            .WithName("GetContractsByEmployeeId");
             

            // GET transactions by contract id
            group.MapGet("/{contractId:int}/transactions", async (IMediator mediator, int contractId, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetContractTransactions.Query(contractId), ct);
                return Results.Ok(result);
            })
            .WithName("GetContractTransactions");
             

            // POST create
            group.MapPost("/", async (
                IMediator mediator,
                CreateContractDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new CreateContract.Command(dto), ct);
                return Results.Created($"/basics/contracts/{id}", new { id });
            })
            .WithName("CreateContract");
             

            // PUT update
            group.MapPut("/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateContractDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateContract.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateContract");
             

            // DELETE soft
            group.MapDelete("/{id:int}/soft", async (
                IMediator mediator,
                int id,
                 CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteContract.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteContract");
            


            // ==================== ContractTransaction (Detail) ====================

            // POST add transaction to existing contract
            group.MapPost("/{contractId:int}/transactions", async (
                IMediator mediator,
                int contractId,
                CreateContractTransactionDto dto,
                CancellationToken ct) =>
            {
                var id = await mediator.Send(new AddContractTransaction.Command(contractId, dto), ct);
                return Results.Created($"/basics/contracts/{contractId}/transactions/{id}", new { id });
            })
            .WithName("AddContractTransaction");
            

            // PUT update transaction
            group.MapPut("/transactions/{id:int}", async (
                IMediator mediator,
                int id,
                UpdateContractTransactionDto dto,
                CancellationToken ct) =>
            {
                var fixedDto = dto with { Id = id };
                await mediator.Send(new UpdateContractTransaction.Command(fixedDto), ct);
                return Results.NoContent();
            })
            .WithName("UpdateContractTransaction");
             

            // DELETE soft transaction
            group.MapDelete("/transactions/{id:int}/soft", async (
                IMediator mediator,
                int id,
                CancellationToken ct) =>
            {
                await mediator.Send(new SoftDeleteContractTransaction.Command(id), ct);
                return Results.NoContent();
            })
            .WithName("SoftDeleteContractTransaction");


            // GET valid contracts (active and not expired)
            group.MapGet("/validcontract", async (
                IMediator mediator,
                [FromQuery] int? employeeId = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new GetValidContracts.Query(employeeId), ct);
                return Results.Ok(result);
            })
            .WithName("GetValidContracts");


            // GET valid contracts with their active transactions
            group.MapGet("/valid-with-transactions", async (
                IMediator mediator,
                [FromQuery] int? employeeId = null,
                CancellationToken ct = default) =>
            {
                var result = await mediator.Send(new GetValidContractsWithTransactions.Query(employeeId), ct);
                return Results.Ok(result);
            })
            .WithName("GetValidContractsWithTransactions");

            group.MapGet("/list", async (
    IMediator mediator,
    HttpContext httpContext,
    CancellationToken ct) =>
            {
                var pageNumber = int.Parse(httpContext.Request.Query["pageNumber"].FirstOrDefault() ?? "1");
                var pageSize = int.Parse(httpContext.Request.Query["pageSize"].FirstOrDefault() ?? "20");
                var orderBy = httpContext.Request.Query["orderBy"].FirstOrDefault();
                var orderDirection = httpContext.Request.Query["orderDirection"].FirstOrDefault();

                var filters = httpContext.Request.Query
                    .Where(x => x.Key != "pageNumber"
                                && x.Key != "pageSize"
                                && x.Key != "orderBy"
                                && x.Key != "orderDirection")
                    .ToDictionary(x => x.Key.ToLower(), x => x.Value.ToString());

                var result = await mediator.Send(new GetContractList.Query(
                    pageNumber, pageSize, orderBy, orderDirection, filters), ct);

                return Results.Json(result);
            })
.WithName("GetContractList");

            return routes;
        }
    }
}