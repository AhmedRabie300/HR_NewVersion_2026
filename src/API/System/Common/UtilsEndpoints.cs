using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.common
{
    public static class UtilsEndpoints
    {
        public static IEndpointRouteBuilder MapUtilsEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/utils")
                .WithTags("Utils");

            // Get Next Code
            group.MapGet("/next-code", async (
                [FromQuery] string entityName,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetNextCode.Query(entityName), ct);
                return Results.Ok(new { entityName, nextCode = result });
            })
            .WithName("GetNextCode")
            .WithOpenApi();

            return routes;
        }
    }
}