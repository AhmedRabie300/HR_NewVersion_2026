using API.Helpers;
using Application.System.MasterData.Nationality.Dtos;
using Application.System.MasterData.Nationality.Queries;
using Application.System.MasterData.Nationality.Commands;
using MediatR;

namespace API.system.MasterData;

public static class NationalityEndpoints
{
    public static IEndpointRouteBuilder MapNationalityEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/hr/master-data/nationalities")
            .WithTags("Nationalities");

        // GET /api/hr/master-data/nationalities
        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new ListNationalities.Query(), ct);
            return Results.Ok(result);
        })
        .RequirePermission("Nationalities", "View")
        .WithName("GetAllNationalities");

        // GET /api/hr/master-data/nationalities/paged
        group.MapGet("/paged", async (
            IMediator mediator,
            int pageNumber = 1,
            int pageSize = 20,
            string? searchTerm = null,
            CancellationToken ct = default) =>
        {
            var result = await mediator.Send(
                new GetPagedNationalities.Query(pageNumber, pageSize, searchTerm),
                ct);
            return Results.Ok(result);
        })
        .RequirePermission("Nationalities", "View")
        .WithName("GetPagedNationalities");

      
         group.MapGet("/{id:int}", async (
            IMediator mediator,
            int id,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetNationalityById.Query(id), ct);
            return Results.Ok(result);
        })
        .RequirePermission("Nationalities", "View")
        .WithName("GetNationalityById");

         group.MapPost("/", async (
            IMediator mediator,
            CreateNationalityDto dto,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(new CreateNationality.Command(dto), ct);
            return Results.Created($"/api/hr/master-data/nationalities/{id}", new { id });
        })
        .RequirePermission("Nationalities", "Add")
        .WithName("CreateNationality");

         group.MapPut("/{id:int}", async (
            IMediator mediator,
            int id,
            UpdateNationalityDto dto,
            CancellationToken ct) =>
        {
            var fixedDto = dto with { Id = id };
            await mediator.Send(new UpdateNationality.Command(fixedDto), ct);
            return Results.NoContent();
        })
        .RequirePermission("Nationalities", "Edit")
        .WithName("UpdateNationality");

    
        

        return routes;
    }
}