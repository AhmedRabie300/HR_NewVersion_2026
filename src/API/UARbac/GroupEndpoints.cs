// API/Endpoints/GroupEndpoints.cs
using Application.UARbac.Groups.Commands;
using Application.UARbac.Groups.Dtos;
using Application.UARbac.Groups.Queries;
using MediatR;
using API.Helpers; // أضف ده

namespace API.Endpoints
{
    public static class GroupEndpoints
    {
        public static IEndpointRouteBuilder MapGroupEndpoints(this IEndpointRouteBuilder routes)
        {
            var groups = routes.MapGroup("/groups").WithTags("Groups");

             groups.MapGet("/",
                async (IMediator mediator, CancellationToken ct) =>
                {
                    var result = await mediator.Send(new ListGroups.Query(), ct);
                    return Results.Ok(result);
                })
                .RequirePermission("Usergroups", "View") // صلاحية عرض
                .WithName("GetAllGroups");

             groups.MapGet("/{id:int}",
                async (IMediator mediator, int id, CancellationToken ct) =>
                {
                    var result = await mediator.Send(new GetGroupById.Query(id), ct);
                    return Results.Ok(result);
                })
                .RequirePermission("Usergroups", "View")  
                .WithName("GetGroupById");

             groups.MapGet("/{id:int}/with-users",
                async (IMediator mediator, int id, CancellationToken ct) =>
                {
                    var result = await mediator.Send(new GetGroupWithUsers.Query(id), ct);
                    return Results.Ok(result);
                })
                .RequirePermission("Usergroups", "View")  
                .WithName("GetGroupWithUsers");

             groups.MapPost("/",
                async (IMediator mediator, CreateGroupDto dto, CancellationToken ct) =>
                {
                    var id = await mediator.Send(new CreateGroup.Command(dto), ct);
                    return Results.Created($"/groups/{id}", new { id });
                })
                .RequirePermission("Usergroups", "Add")  
                .WithName("CreateGroup");

             groups.MapPut("/{id:int}",
                async (IMediator mediator, int id, UpdateGroupDto dto, CancellationToken ct) =>
                {
                    var fixedDto = dto with { Id = id };
                    await mediator.Send(new UpdateGroup.Command(fixedDto), ct);
                    return Results.NoContent();
                })
                .RequirePermission("Usergroups", "Edit")  
                .WithName("UpdateGroup");

             groups.MapDelete("/{id:int}",
                async (IMediator mediator, int id, CancellationToken ct) =>
                {
                    var result = await mediator.Send(new DeleteGroup.Command(id), ct);
                    return result ? Results.NoContent() : Results.NotFound();
                })
                .RequirePermission("Usergroups", "Delete")  
                .WithName("DeleteGroup");

            return routes;
        }
    }
}