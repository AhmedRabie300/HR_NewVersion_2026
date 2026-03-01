using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VenusHR.API.Helpers;
using VenusHR.Application.Common.Interfaces;
using VenusHR.Application.Common.Interfaces.HR_Master.Banks;
using VenusHR.Application.Common.Interfaces.HR_Master.Education;
using VenusHR.Application.Common.Interfaces.HR_Master.MaritalStatus;
using VenusHR.Application.Common.Interfaces.HR_Master.Religions;
using VenusHR.Core.Master;
using WorkFlow_EF;

namespace VenusHR.API.Endpoints.LookupEndPoints
{
    public static class HRMasterLookupsEndpoints
    {
        public static void MapHRMasterLookupsEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/hr");

            // =============== BLOOD GROUPS ===============
            group.MapGet("/bloodgroups", GetAllBloodGroups)
                .RequirePermission("BloodGroups", "View");
            group.MapGet("/bloodgroups/{id}", GetBloodGroupById)
                .RequirePermission("BloodGroups", "View");
            group.MapPost("/bloodgroups", CreateBloodGroup)
                .RequirePermission("BloodGroups", "Add");
            group.MapPut("/bloodgroups/{id}", UpdateBloodGroup)
                .RequirePermission("BloodGroups", "Edit");
            group.MapDelete("/bloodgroups/{id}", DeleteBloodGroup)
                .RequirePermission("BloodGroups", "Delete");

            // =============== NATIONALITIES ===============
            group.MapGet("/nationalities", GetAllNationalities)
                .RequirePermission("Nationalities", "View");
            group.MapGet("/nationalities/{id}", GetNationalityById)
                .RequirePermission("Nationalities", "View");
            group.MapPost("/nationalities", CreateNationality)
                .RequirePermission("Nationalities", "Add");
            group.MapPut("/nationalities/{id}", UpdateNationality)
                .RequirePermission("Nationalities", "Edit");

            // =============== BANKS ===============
            group.MapGet("/banks", GetAllBanks)
                .RequirePermission("Banks", "View");
            group.MapGet("/banks/{id}", GetBankById)
                .RequirePermission("Banks", "View");
            group.MapPost("/banks", CreateBank)
                .RequirePermission("Banks", "Add");
            group.MapPut("/banks/{id}", UpdateBank)
                .RequirePermission("Banks", "Edit");

            // =============== RELIGIONS ===============
            group.MapGet("/religions", GetAllReligions)
                .RequirePermission("Religions", "View");
            group.MapGet("/religions/{id}", GetReligionById)
                .RequirePermission("Religions", "View");

            // =============== MARITAL STATUS ===============
            group.MapGet("/marital-status", GetAllMaritalStatus)
                .RequirePermission("MaritalStatus", "View");
            group.MapGet("/marital-status/{id}", GetMaritalStatusById)
                .RequirePermission("MaritalStatus", "View");

            // =============== EDUCATIONS ===============
            group.MapGet("/educations", GetAllEducations)
                .RequirePermission("Educations", "View");
            group.MapGet("/educations/{id}", GetEducationById)
                .RequirePermission("Educations", "View");
            group.MapPost("/educations", CreateEducation)
                .RequirePermission("Educations", "Add");
            group.MapPut("/educations/{id}", UpdateEducation)
                .RequirePermission("Educations", "Edit");

            // =============== PROFESSIONS ===============
            group.MapGet("/professions", GetAllProfessions)
                .RequirePermission("Professions", "View");
            group.MapGet("/professions/{id}", GetProfessionById)
                .RequirePermission("Professions", "View");

            // =============== CITIES ===============
            group.MapGet("/cities", GetAllCities)
                .RequirePermission("Cities", "View");
            group.MapGet("/cities/{id}", GetCityById)
                .RequirePermission("Cities", "View");

            // =============== COMPANIES ===============
            group.MapGet("/companies", GetAllCompanies)
                .RequirePermission("Companies", "View");
            group.MapGet("/companies/{id}", GetCompanyById)
                .RequirePermission("Companies", "View");

            // =============== DEPARTMENTS ===============
            group.MapGet("/departments", GetAllDepartments)
                .RequirePermission("Departments", "View");
            group.MapGet("/departments/{id}", GetDepartmentById)
                .RequirePermission("Departments", "View");
            group.MapGet("/departments/by-company/{companyId}", GetDepartmentsByCompany)
                .RequirePermission("Departments", "View");

            // =============== POSITIONS ===============
            group.MapGet("/positions", GetAllPositions)
                .RequirePermission("Positions", "View");
            group.MapGet("/positions/{id}", GetPositionById)
                .RequirePermission("Positions", "View");

            // =============== PROJECTS ===============
            group.MapGet("/projects", GetAllProjects)
                .RequirePermission("Projects", "View");
            group.MapGet("/projects/{id}", GetProjectById)
                .RequirePermission("Projects", "View");

            // =============== LOCATIONS ===============
            group.MapGet("/locations", GetAllLocations)
                .RequirePermission("Locations", "View");
            group.MapGet("/locations/{id}", GetLocationById)
                .RequirePermission("Locations", "View");
            group.MapGet("/locations/by-city/{cityId}", GetLocationsByCity)
                .RequirePermission("Locations", "View");

            // =============== VACATION TYPES ===============
            group.MapGet("/vacation-types", GetAllVacationTypes)
                .RequirePermission("VacationTypes", "View");
            group.MapGet("/vacation-types/{id}", GetVacationTypeById)
                .RequirePermission("VacationTypes", "View");

            // =============== CONTRACTS ===============
            group.MapGet("/contracts", GetAllContracts)
                .RequirePermission("Contracts", "View");
            group.MapGet("/contracts/{id}", GetContractById)
                .RequirePermission("Contracts", "View");
            group.MapPost("/contracts", CreateContract)
                .RequirePermission("Contracts", "Add");
            group.MapPut("/contracts/{id}", UpdateContract)
                .RequirePermission("Contracts", "Edit");

            // =============== SYSTEM LOOKUPS ===============
            group.MapGet("/lookups", GetSystemLookups)
                .RequirePermission("Lookups", "View");
            group.MapGet("/lookups/by-type/{lookupType}", GetLookupByType)
                .RequirePermission("Lookups", "View");
            group.MapGet("/lookups/search", SearchLookups)
                .RequirePermission("Lookups", "View");
        }

        // =============== BLOOD GROUPS HANDLERS ===============
        private static async Task<IResult> GetAllBloodGroups(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllBloodGroupsQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetBloodGroupById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetBloodGroupByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        private static async Task<IResult> CreateBloodGroup(
            [FromBody] hrs_BloodGroups bloodGroup,
            [FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new CreateBloodGroupCommand(bloodGroup));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> UpdateBloodGroup(
            int id,
            [FromBody] hrs_BloodGroups bloodGroup,
            [FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new UpdateBloodGroupCommand(id, bloodGroup));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> DeleteBloodGroup(
            int id,
            [FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new DeleteBloodGroupCommand(id));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        // =============== NATIONALITIES HANDLERS ===============
        private static async Task<IResult> GetAllNationalities(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllNationalitiesQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetNationalityById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetNationalityByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        private static async Task<IResult> CreateNationality(
            [FromBody] sys_Nationalities nationality,
            [FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new CreateNationalityCommand(nationality));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> UpdateNationality(
            int id,
            [FromBody] sys_Nationalities nationality,
            [FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new UpdateNationalityCommand(id, nationality));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        // =============== BANKS HANDLERS ===============
        private static async Task<IResult> GetAllBanks(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllBanksQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetBankById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetBankByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        private static async Task<IResult> CreateBank(
            [FromBody] sys_Banks bank,
            [FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new CreateBankCommand(bank));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> UpdateBank(
            int id,
            [FromBody] sys_Banks bank,
            [FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new UpdateBankCommand(id, bank));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        // =============== RELIGIONS HANDLERS ===============
        private static async Task<IResult> GetAllReligions(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllReligionsQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetReligionById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetReligionByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        // =============== MARITAL STATUS HANDLERS ===============
        private static async Task<IResult> GetAllMaritalStatus(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllMaritalStatusQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetMaritalStatusById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetMaritalStatusByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        // =============== EDUCATIONS HANDLERS ===============
        private static async Task<IResult> GetAllEducations(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllEducationsQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetEducationById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetEducationByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        private static async Task<IResult> CreateEducation(
            [FromBody] hrs_Educations education,
            [FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new CreateEducationCommand(education));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> UpdateEducation(
            int id,
            [FromBody] hrs_Educations education,
            [FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new UpdateEducationCommand(id, education));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        // =============== PROFESSIONS HANDLERS ===============
        private static async Task<IResult> GetAllProfessions(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllProfessionsQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetProfessionById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetProfessionByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        // =============== CITIES HANDLERS ===============
        private static async Task<IResult> GetAllCities(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllCitiesQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetCityById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetCityByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        // =============== COMPANIES HANDLERS ===============
        private static async Task<IResult> GetAllCompanies(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllCompaniesQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetCompanyById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetCompanyByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        // =============== DEPARTMENTS HANDLERS ===============
        private static async Task<IResult> GetAllDepartments(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllDepartmentsQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetDepartmentById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetDepartmentByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        private static async Task<IResult> GetDepartmentsByCompany(
            int companyId,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetDepartmentsByCompanyQuery(companyId, lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        // =============== POSITIONS HANDLERS ===============
        private static async Task<IResult> GetAllPositions(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllPositionsQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetPositionById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetPositionByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        // =============== PROJECTS HANDLERS ===============
        private static async Task<IResult> GetAllProjects(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllProjectsQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetProjectById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetProjectByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        // =============== LOCATIONS HANDLERS ===============
        private static async Task<IResult> GetAllLocations(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllLocationsQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetLocationById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetLocationByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        private static async Task<IResult> GetLocationsByCity(
            int cityId,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetLocationsByCityQuery(cityId, lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        // =============== VACATION TYPES HANDLERS ===============
        private static async Task<IResult> GetAllVacationTypes(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllVacationTypesQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetVacationTypeById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetVacationTypeByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        // =============== CONTRACTS HANDLERS ===============
        private static async Task<IResult> GetAllContracts(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetAllContractsQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetContractById(
            int id,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetContractByIdQuery(id, lang));
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        }

        private static async Task<IResult> CreateContract(
            [FromBody] hrs_Contracts contract,
            [FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new CreateContractCommand(contract));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> UpdateContract(
            int id,
            [FromBody] hrs_Contracts contract,
            [FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new UpdateContractCommand(id, contract));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        // =============== SYSTEM LOOKUPS HANDLERS ===============
        private static async Task<IResult> GetSystemLookups(
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetSystemLookupsQuery(lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> GetLookupByType(
            string lookupType,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new GetLookupByTypeQuery(lookupType, lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }

        private static async Task<IResult> SearchLookups(
            [FromQuery] string searchTerm,
            [FromQuery] string? lookupType,
            [FromServices] IMediator mediator,
            [FromQuery] int lang = 0)
        {
            var result = await mediator.Send(new SearchLookupsQuery(searchTerm, lookupType, lang));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }
    }
}