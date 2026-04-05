using Application.UARbac.UserGroups.Commands;
using Application.UARbac.UserGroups.Dtos;
using Application.UARbac.UserGroups.Queries;
using MediatR;

namespace API.Endpoints
{
    public static class UserGroupEndpoints
    {
        public static IEndpointRouteBuilder MapUserGroupEndpoints(this IEndpointRouteBuilder routes)
        {
            var userGroups = routes.MapGroup("/user-groups").WithTags("UserGroups");
 
            userGroups.MapGet("/users/{userId:int}",
                async (IMediator mediator, int userId, CancellationToken ct) =>
                {
                    var result = await mediator.Send(new GetUserGroups.Query(userId), ct);
                    return Results.Ok(result);
                })
               // .RequirePermission("SystemGroups", "View") 
                .WithName("GetUserGroups");

 
            userGroups.MapGet("/groups/{groupId:int}",
                async (IMediator mediator, int groupId, CancellationToken ct) =>
                {
                    var result = await mediator.Send(new GetGroupUsers.Query(groupId), ct);
                    return Results.Ok(result);
                })
               // .RequirePermission("SystemGroups", "View")  
                .WithName("GetGroupUsers");

  
            userGroups.MapPost("/",
                async (IMediator mediator, AddUserToGroupDto dto, CancellationToken ct) =>
                {
                    var id = await mediator.Send(new AddUserToGroup.Command(dto), ct);
                    return Results.Created($"/user-groups/{id}", new { id });
                })
               // .RequirePermission("SystemGroups", "Edit")  
                .WithName("AddUserToGroup");

         

            return routes;
        }
    }
}