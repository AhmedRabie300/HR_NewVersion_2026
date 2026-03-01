using global::VenusHR.API.Helpers;
using global::VenusHR.Application.Common.DTOs.Forms;
using Microsoft.AspNetCore.Mvc;
using VenusHR.API.Helpers;
using MediatR;
using VenusHR.Application.Common.Interfaces.Forms;  

namespace VenusHR.API.Endpoints.Forms
{
    public static class FormsControlEndpoints
    {
        public static void MapFormsControlEndpoints(this IEndpointRouteBuilder routes)
        {
            // Queries
            routes.MapGet("/api/GetAllControls", GetAllControls)
                .RequirePermission("Forms", "View");

            routes.MapGet("/api/GetControlById/{id:int}", GetControlById)
                .RequirePermission("Forms", "View");

            routes.MapGet("/api/GetControlsByFormId/{formId:int}", GetControlsByFormId);

            routes.MapGet("/api/forms/{formId:int}/controls/visible", GetVisibleControlsByFormId);

             routes.MapPost("/api/forms-controls", CreateControl)
                .RequirePermission("Forms", "Add");

            routes.MapPost("/api/forms-controls/bulk", CreateBulkControls)
                .RequirePermission("Forms", "Add");

            routes.MapPut("/api/forms-controls/{id:int}", UpdateControl)
                .RequirePermission("Forms", "Edit");

            routes.MapGet("/api/forms/{formId:int}/controls/reorder", ReorderControls)
                .RequirePermission("Forms", "Edit");

            routes.MapDelete("/api/forms-controls/{id:int}", DeleteControl)
                .RequirePermission("Forms", "Delete");

            routes.MapDelete("/api/forms/{formId:int}/controls", DeleteControlsByFormId)
                .RequirePermission("Forms", "Delete");
        }

         private static async Task<IResult> GetAllControls([FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new FormsControlQueries.GetAllControlsQuery());
                return Results.Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> GetControlById(int id, [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new FormsControlQueries.GetControlByIdQuery(id));
                if (result == null)
                    return Results.NotFound(new { success = false, message = "Control not found" });

                return Results.Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> GetControlsByFormId(int formId, [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new FormsControlQueries.GetControlsByFormIdQuery(formId));
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> GetVisibleControlsByFormId(int formId, [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new FormsControlQueries.GetVisibleControlsByFormIdQuery(formId));
                return Results.Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

         private static async Task<IResult> CreateControl(
            [FromBody] CreateFormsControlDto dto,
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new FormsControlCommands.CreateControlCommand(dto));
                return Results.Created($"/api/forms-controls/{result.Id}", new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> CreateBulkControls(
            [FromBody] List<CreateFormsControlDto> dtos,
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new FormsControlCommands.CreateBulkControlsCommand(dtos));
                return Results.Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> UpdateControl(
            int id,
            [FromBody] UpdateFormsControlDto dto,
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new FormsControlCommands.UpdateControlCommand(id, dto));
                return Results.Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> DeleteControl(int id, [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new FormsControlCommands.DeleteControlCommand(id));
                return Results.Ok(new
                {
                    success = result,
                    message = result ? "Control deleted successfully" : "Control not found"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> DeleteControlsByFormId(int formId, [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new FormsControlCommands.DeleteControlsByFormIdCommand(formId));
                return Results.Ok(new
                {
                    success = result,
                    message = result ? "Controls deleted successfully" : "No controls found"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> ReorderControls(
            int formId,
            [FromBody] Dictionary<int, int> controlRanks,
            [FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new FormsControlCommands.ReorderControlsCommand(formId, controlRanks));
                return Results.Ok(new
                {
                    success = result,
                    message = result ? "Controls reordered successfully" : "Reorder failed"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }
    }
}