using Microsoft.AspNetCore.Mvc;
using VenusHR.API.Helpers;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.API.Endpoints.LookupEndPoints
{
    public static class HRMasterEndpoints
    {
        public static void MapHRMasterEndpoints(this WebApplication app)
        {
            app.MapGet("/api/hr-master/lookups/all/{lang}", GetAllMasterData)
                .RequirePermission("Lookups", "View");

            app.MapGet("/api/hr-master/lookups/by-type/{lookupType}/{lang}", GetLookupByType)
                .RequirePermission("Lookups", "View");

            app.MapGet("/api/hr-master/lookups/search", SearchLookups)
                .RequirePermission("Lookups", "View");

            app.MapGet("/api/hr-master/cities/all/{lang}", GetAllCities)
                .RequirePermission("Cities", "View");

            app.MapGet("/api/hr-master/cities/{id:int}/{lang}", GetCityById)
                .RequirePermission("Cities", "View");

            app.MapGet("/api/hr-master/nationalities/all/{lang}", GetAllNationalities)
                .RequirePermission("Nationalities", "View");

            app.MapGet("/api/hr-master/nationalities/{id:int}/{lang}", GetNationalityById)
                .RequirePermission("Nationalities", "View");

            app.MapPost("/api/hr-master/nationalities", CreateNationality)
                .RequirePermission("Nationalities", "Add");

            app.MapPut("/api/hr-master/nationalities/{id:int}", UpdateNationality)
                .RequirePermission("Nationalities", "Edit");

            app.MapGet("/api/hr-master/banks/all/{lang}", GetAllBanks)
                .RequirePermission("Banks", "View");

            app.MapGet("/api/hr-master/banks/{id:int}/{lang}", GetBankById)
                .RequirePermission("Banks", "View");

            app.MapPost("/api/hr-master/banks", CreateBank)
                .RequirePermission("Banks", "Add");

            app.MapPut("/api/hr-master/banks/{id:int}", UpdateBank)
                .RequirePermission("Banks", "Edit");

            app.MapGet("/api/hr-master/religions/all/{lang}", GetAllReligions)
                .RequirePermission("Religions", "View");

            app.MapGet("/api/hr-master/religions/{id:int}/{lang}", GetReligionById)
                .RequirePermission("Religions", "View");

            app.MapGet("/api/hr-master/marital-status/all/{lang}", GetAllMaritalStatus)
                .RequirePermission("MaritalStatus", "View");

            app.MapGet("/api/hr-master/marital-status/{id:int}/{lang}", GetMaritalStatusById)
                .RequirePermission("MaritalStatus", "View");

            app.MapGet("/api/hr-master/blood-groups/all/{lang}", GetAllBloodGroups)
                .RequirePermission("BloodGroups", "View");

            app.MapGet("/api/hr-master/blood-groups/{id:int}/{lang}", GetBloodGroupById)
                .RequirePermission("BloodGroups", "View");

            app.MapPost("/api/hr-master/blood-groups", CreateBloodGroup)
                .RequirePermission("BloodGroups", "Add");

            app.MapPut("/api/hr-master/blood-groups/{id:int}", UpdateBloodGroup)
                .RequirePermission("BloodGroups", "Edit");

            app.MapDelete("/api/hr-master/blood-groups/{id:int}", DeleteBloodGroup)
                .RequirePermission("BloodGroups", "Delete");

            app.MapGet("/api/hr-master/educations/all/{lang}", GetAllEducations)
                .RequirePermission("Educations", "View");

            app.MapGet("/api/hr-master/educations/{id:int}/{lang}", GetEducationById)
                .RequirePermission("Educations", "View");

            app.MapPost("/api/hr-master/educations", CreateEducation)
                .RequirePermission("Educations", "Add");

            app.MapPut("/api/hr-master/educations/{id:int}", UpdateEducation)
                .RequirePermission("Educations", "Edit");

            app.MapGet("/api/hr-master/professions/all/{lang}", GetAllProfessions)
                .RequirePermission("Professions", "View");

            app.MapGet("/api/hr-master/professions/{id:int}/{lang}", GetProfessionById)
                .RequirePermission("Professions", "View");

            app.MapGet("/api/hr-master/companies/all/{lang}", GetAllCompanies)
                .RequirePermission("Companies", "View");

            app.MapGet("/api/hr-master/companies/{id:int}/{lang}", GetCompanyById)
                .RequirePermission("Companies", "View");

            app.MapPost("/api/hr-master/employees/new", SaveNewEmployeeForm)
                .RequirePermission("Employees", "Add");

            app.MapGet("/api/hr-master/health", TestConnection);
            app.MapGet("/api/test-jwt-simple", TestJwtSimple);
        }

        private static async Task<IResult> GetAllMasterData(
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetSystemLookupsAsync(lang);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetLookupByType(
            string lookupType,
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetLookupByTypeAsync(lookupType, lang);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> SearchLookups(
            [FromQuery] string searchTerm,
            [FromServices] IHRMaster service,
            [FromQuery] string? lookupType = null,
            [FromQuery] int lang = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(searchTerm))
                    return Results.BadRequest(new { error = "Search term is required" });

                var result = await service.SearchLookupsAsync(searchTerm, lookupType, lang);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetAllCities(
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetAllCitiesAsync(lang);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetCityById(
            int id,
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetCityByIdAsync(id, lang);
                if (result.ErrorCode == 0)
                    return Results.NotFound(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetAllNationalities(
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetAllNationalitiesAsync(lang);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetNationalityById(
            int id,
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetNationalityByIdAsync(id, lang);
                if (result.ErrorCode == 0)
                    return Results.NotFound(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> CreateNationality(
            [FromBody] sys_Nationalities nationality,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.CreateNationalityAsync(nationality);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });

                var createdNationality = (sys_Nationalities)result.ResultObject;
                return Results.Created($"/api/hr-master/nationalities/{createdNationality.ID}/{0}", createdNationality);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> UpdateNationality(
            int id,
            [FromBody] sys_Nationalities nationality,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.UpdateNationalityAsync(id, nationality);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetAllBanks(
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetAllBanksAsync(lang);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetBankById(
            int id,
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetBankByIdAsync(id, lang);
                if (result.ErrorCode == 0)
                    return Results.NotFound(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> CreateBank(
            [FromBody] sys_Banks bank,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.CreateBankAsync(bank);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });

                var createdBank = (sys_Banks)result.ResultObject;
                return Results.Created($"/api/hr-master/banks/{createdBank.ID}/{0}", createdBank);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> UpdateBank(
            int id,
            [FromBody] sys_Banks bank,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.UpdateBankAsync(id, bank);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetAllReligions(
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetAllReligionsAsync(lang);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetReligionById(
            int id,
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetReligionByIdAsync(id, lang);
                if (result.ErrorCode == 0)
                    return Results.NotFound(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetAllMaritalStatus(
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetAllMaritalStatusAsync(lang);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetMaritalStatusById(
            int id,
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetMaritalStatusByIdAsync(id, lang);
                if (result.ErrorCode == 0)
                    return Results.NotFound(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetAllBloodGroups(
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetAllBloodGroupsAsync(lang);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetBloodGroupById(
            int id,
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetBloodGroupByIdAsync(id, lang);
                if (result.ErrorCode == 0)
                    return Results.NotFound(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> CreateBloodGroup(
            [FromBody] hrs_BloodGroups bloodGroup,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.CreateBloodGroupAsync(bloodGroup);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });

                var createdBloodGroup = (hrs_BloodGroups)result.ResultObject;
                return Results.Created($"/api/hr-master/blood-groups/{createdBloodGroup.ID}/{0}", createdBloodGroup);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> UpdateBloodGroup(
            int id,
            [FromBody] hrs_BloodGroups bloodGroup,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.UpdateBloodGroupAsync(id, bloodGroup);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> DeleteBloodGroup(
            int id,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.DeleteBloodGroupAsync(id);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });

                return Results.Ok(new { success = true, message = "Blood group deleted successfully" });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetAllEducations(
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetAllEducationsAsync(lang);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetEducationById(
            int id,
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetEducationByIdAsync(id, lang);
                if (result.ErrorCode == 0)
                    return Results.NotFound(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> CreateEducation(
            [FromBody] hrs_Educations education,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.CreateEducationAsync(education);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });

                var createdEducation = (hrs_Educations)result.ResultObject;
                return Results.Created($"/api/hr-master/educations/{createdEducation.ID}/{0}", createdEducation);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> UpdateEducation(
            int id,
            [FromBody] hrs_Educations education,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.UpdateEducationAsync(id, education);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetAllProfessions(
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetAllProfessionsAsync(lang);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetProfessionById(
            int id,
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetProfessionByIdAsync(id, lang);
                if (result.ErrorCode == 0)
                    return Results.NotFound(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetAllCompanies(
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetAllCompaniesAsync(lang);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> GetCompanyById(
            int id,
            int lang,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.GetCompanyByIdAsync(id, lang);
                if (result.ErrorCode == 0)
                    return Results.NotFound(new { error = result.ErrorMessage });
                return Results.Ok(result.ResultObject);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> SaveNewEmployeeForm(
            [FromBody] Hrs_NewEmployee newEmployee,
            [FromServices] IHRMaster service)
        {
            try
            {
                var result = await service.SaveNewEmployeeFormAsync(newEmployee);
                if (result.ErrorCode == 0)
                    return Results.BadRequest(new { error = result.ErrorMessage });

                return Results.Ok(new
                {
                    success = true,
                    message = "Employee saved successfully",
                    data = result.ResultObject
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static IResult TestJwtSimple(HttpContext context)
        {
            var items = context.Items.Keys.Cast<object>()
                .ToDictionary(k => k.ToString() ?? "", k => context.Items[k]?.ToString() ?? "null");

            return Results.Ok(new
            {
                Message = "JWT Simple Test",
                UserId = context.Items["UserId"] as string ?? "Not found",
                UserCode = context.Items["UserCode"] as string ?? "Not found",
                UserName = context.Items["UserName"] as string ?? "Not found",
                IsAdmin = context.Items["IsAdmin"] as string ?? "Not found",
                IsAuthenticated = context.Items["IsAuthenticated"] as bool? ?? false,
                AllItems = items,
                Timestamp = DateTime.UtcNow
            });
        }

        private static IResult TestConnection()
        {
            return Results.Ok(new
            {
                status = "API is running",
                timestamp = DateTime.UtcNow,
                version = "1.0.0"
            });
        }
    }
}