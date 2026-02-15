
using global::VenusHR.API.Helpers;
using global::VenusHR.Application.Common.DTOs.Forms;
using global::VenusHR.Application.Common.Interfaces.Forms;
 using Microsoft.AspNetCore.Mvc;
using VenusHR.API.Helpers;
 
namespace VenusHR.API.Endpoints.Forms
     
{
    public static class FormsControlEndpoints
    {
        public static void MapFormsControlEndpoints(this WebApplication app)
        {
             app.MapGet("/api/GetAllControls", GetAllControls)
                .RequirePermission("Forms", "View");

            app.MapGet("/api/GetControlById/{id:int}", GetControlById)
                .RequirePermission("Forms", "View");

            app.MapGet("/api/GetControlsByFormId/{formId:int}", GetControlsByFormId);
                //.RequirePermission("Forms", "View");

            app.MapGet("/api/forms/{formId:int}/controls/visible", GetVisibleControlsByFormId);
                //.RequirePermission("Forms", "View");

             app.MapPost("/api/forms-controls", CreateControl)
                .RequirePermission("Forms", "Add");

            app.MapPost("/api/forms-controls/bulk", CreateBulkControls)
                .RequirePermission("Forms", "Add");

             app.MapPut("/api/forms-controls/{id:int}", UpdateControl)
                .RequirePermission("Forms", "Edit");

            app.MapGet("/api/forms/{formId:int}/controls/reorder", ReorderControls)
                .RequirePermission("Forms", "Edit");

             app.MapDelete("/api/forms-controls/{id:int}", DeleteControl)
                .RequirePermission("Forms", "Delete");

            app.MapDelete("/api/forms/{formId:int}/controls", DeleteControlsByFormId)
                .RequirePermission("Forms", "Delete");
        }

        private static async Task<IResult> GetAllControls(
            [FromServices] IFormsControlService service)
        {
            try
            {
                var result = await service.GetAllControlsAsync();
                return Results.Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> GetControlById(
            int id,
            [FromServices] IFormsControlService service)
        {
            try
            {
                var result = await service.GetControlByIdAsync(id);
                if (result == null)
                    return Results.NotFound(new { success = false, message = "Control not found" });

                return Results.Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

      private static async Task<IResult> GetControlsByFormId(
    int formId,
    [FromServices] IFormsControlService service)
        {
            try
            {
                var result = await service.GetFormControlsStructureAsync(formId);
                return Results.Ok(result);  // ✅ من غير wrap
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }
        private static async Task<IResult> GetVisibleControlsByFormId(
            int formId,
            [FromServices] IFormsControlService service)
        {
            try
            {
                var result = await service.GetVisibleControlsByFormIdAsync(formId);
                return Results.Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> CreateControl(
            [FromBody] CreateFormsControlDto dto,
            [FromServices] IFormsControlService service)
        {
            try
            {
                var result = await service.CreateControlAsync(dto);
                return Results.Created($"/api/forms-controls/{result.Id}", new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> CreateBulkControls(
            [FromBody] List<CreateFormsControlDto> dtos,
            [FromServices] IFormsControlService service)
        {
            try
            {
                var result = await service.CreateBulkControlsAsync(dtos);
                return Results.Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> UpdateControl(
            int id,
            [FromBody] UpdateFormsControlDto dto,
            [FromServices] IFormsControlService service)
        {
            try
            {
                var result = await service.UpdateControlAsync(id, dto);
                return Results.Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> DeleteControl(
            int id,
            [FromServices] IFormsControlService service)
        {
            try
            {
                var result = await service.SoftDeleteControlAsync(id);
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

        private static async Task<IResult> DeleteControlsByFormId(
            int formId,
            [FromServices] IFormsControlService service)
        {
            try
            {
                var result = await service.DeleteControlsByFormIdAsync(formId);
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
            [FromServices] IFormsControlService service)
        {
            try
            {
                var result = await service.ReorderControlsAsync(formId, controlRanks);
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