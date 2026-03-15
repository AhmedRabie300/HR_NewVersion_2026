using MediatR;
using Application.UARbac.Menus.Commands;
using Application.UARbac.Menus.Dtos;
using Application.UARbac.Menus.Queries;

namespace API.UARbac
{
    public static class MenuEndpoints
    {
        public static IEndpointRouteBuilder MapMenuEndpoints(this IEndpointRouteBuilder routes)
        {
            var menus = routes.MapGroup("/menus").WithTags("Menus");

             
 

             menus.MapGet("/tree",
                async (IMediator mediator, bool includeHidden = false, CancellationToken ct = default) =>
                {
                    var result = await mediator.Send(new GetMenuTree.Query(includeHidden), ct);
                    return Results.Ok(result);
                })
                .WithName("GetMenuTree");

             menus.MapGet("/{id:int}",
                async (IMediator mediator, int id, CancellationToken ct) =>
                {
                    var result = await mediator.Send(new GetMenuById.Query(id), ct);
                    return Results.Ok(result);
                })
                .WithName("GetMenuById");

             menus.MapGet("/users/{userId:int}",
                async (IMediator mediator, int userId, bool visibleOnly = true, CancellationToken ct = default) =>
                {
                    var result = await mediator.Send(new GetUserMenus.Query(userId, visibleOnly), ct);
                    return Results.Ok(result);
                })
                .WithName("GetUserMenus");

             menus.MapPost("/",
                async (IMediator mediator, CreateMenuDto dto, CancellationToken ct) =>
                {
                    var id = await mediator.Send(new CreateMenu.Command(dto), ct);
                    return Results.Created($"/menus/{id}", new { id });
                })
                .WithName("CreateMenu");

             menus.MapPut("/{id:int}",
                async (IMediator mediator, int id, UpdateMenuDto dto, CancellationToken ct) =>
                {
                    var fixedDto = dto with { Id = id };
                    await mediator.Send(new UpdateMenu.Command(fixedDto), ct);
                    return Results.NoContent();
                })
                .WithName("UpdateMenu");
 
        

            return routes;
        }
    }
}