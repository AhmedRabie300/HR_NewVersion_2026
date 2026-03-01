using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VenusHR.Application.Common.DTOs.Login;
using VenusHR.Application.Common.Interfaces.Login;
using VenusHR.Application.Common.Interfaces.Login.Command;
using VenusHR.Core.Login;

namespace VenusHR.API.Endpoints
{
    public static class LoginEndpoints
    {
         public static void MapLoginEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/auth");
            MapLoginEndpoints(group);
        }

         public static void MapLoginEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost("/LoginOld", LoginOld).AllowAnonymous();
            group.MapPost("/login", LoginNew).AllowAnonymous();
             group.MapGet("/status", Status).AllowAnonymous();       
            group.MapPost("/logout", Logout).AllowAnonymous();      
        }

        


        private static async Task<IResult> LoginNew(
           [FromBody] LoginRequestDto request,
           [FromQuery] int lang,
           [FromServices] IMediator mediator,  
           CancellationToken ct)
        {
             var result = await mediator.Send(new LoginCommand(request, lang), ct);

             if (result.Success)
                return Results.Ok(result);
            return Results.Unauthorized();
        }

 
        private static async Task<IResult> LoginOld(
           [FromBody] Sys_Users user,
           [FromQuery] int lang,
           [FromServices] ILoginServices loginService)   
        {
            var result = loginService.Login(
                user.Code,
                user.Password,
                lang,
                user.DeviceToken);
            return Results.Ok(result);
        }

        private static IResult Status()
        {
            return Results.Ok(new { Status = "API is running" });
        }

        private static IResult Logout()
        {
            return Results.Ok(new { Message = "Logged out" });
        }
    }
}