// API/Endpoints/SearchEndpoints.cs
using Application.System.Search.Dtos;
using Application.System.Search.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints
{
    public static class SearchEndpoints
    {
        public static IEndpointRouteBuilder MapSearchEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/search")
                .WithTags("Search");

            // GET /api/search/columns/{searchID} - جلب أعمدة البحث
             group.MapGet("/columns/{searchID:int}", async (
                IMediator mediator,
                int searchID,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetSearchColumns.Query(searchID), ct);
                return Results.Ok(result);
            })
            .WithName("GetSearchColumns")
            .WithDescription("Get search columns (criteria and view) for a specific search ID");

            // POST /api/search/execute - تنفيذ البحث
            group.MapPost("/execute", async (
                IMediator mediator,
                [FromBody] SearchExecuteRequestDto request,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(new ExecuteSearch.Query(request), ct);
                return Results.Ok(result);
            })
            .WithName("ExecuteSearch")
            .WithDescription("Execute search with criteria");

            return routes;
        }
    }
}