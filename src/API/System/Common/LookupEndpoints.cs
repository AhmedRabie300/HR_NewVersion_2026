using Application.Common;
using Application.Common.Lookups;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Common.Endpoints
{
    public static class LookupEndpoints
    {
        public static IEndpointRouteBuilder MapLookupEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/lookup")
                .WithTags("Lookup");

            // Get Lookup by Entity Name with optional criteria
            group.MapGet("/{entityName}", async (
                string entityName,
                [FromQuery] string? criteria,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetLookup.Query(entityName, criteria), ct);
                return Results.Ok(result);
            })
            .WithName("GetLookup");
      

            return routes;
        }
    }
}