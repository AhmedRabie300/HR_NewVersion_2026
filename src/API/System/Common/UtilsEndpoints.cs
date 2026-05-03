using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.System.common
{
    public static class UtilsEndpoints
    {
        public static IEndpointRouteBuilder MapUtilsEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("utils")
                .WithTags("Utils");

            // Get Next Code
            group.MapGet("/next-code", async (
                [FromQuery] string entityName,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetNextCode.Query(entityName), ct);
                return Results.Ok(result);
            })
            .WithName("GetNextCode");

            return routes;
        }
    }
}