using Microsoft.AspNetCore.Mvc;
using VenusHR.Application.Common.DTOs.Login;
using VenusHR.Application.Common.Interfaces.Login;
using VenusHR.Core.Login;

public static class LoginEndpoints
{
    public static void MapLoginEndpoints(this WebApplication app)
    {
         app.MapPost("/api/auth/LoginOld", LoginOld);

         app.MapPost("/api/auth/login", LoginNew);

        app.MapGet("/api/auth/status", Status);
        app.MapPost("/api/auth/logout", Logout);
    }

     private static async Task<IResult> LoginNew(
        [FromBody] LoginRequestDto request,
        [FromQuery] int lang,
        ILoginServices loginService)
    {
        var result = await loginService.LoginAsync(request, lang);

        if (result.Success)
            return Results.Ok(result);

        return Results.Unauthorized();
    }

     private static async Task<IResult> LoginOld(
        [FromBody] Sys_Users user,   
        [FromQuery] int lang,
        ILoginServices loginService)
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