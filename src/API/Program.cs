using API.Common.Endpoints;
using API.Common.Middleware;
using API.Common.Swagger;
using API.Common.Versioning;
using API.Endpoints;
using API.Helpers;
using API.System.common;
using API.System.HRS;
using API.System.HRS.Basics.ContractsTypes;
using API.System.HRS.Basics.FiscalPeriod;
using API.System.HRS.Basics.FiscalTransactions;
using API.System.HRS.Basics.Grades;
using API.System.HRS.VacationAndEndOfService;
using API.System.MasterData;
using API.UARbac;
using Application;
using Asp.Versioning;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ??
            throw new InvalidOperationException("JWT SecretKey not configured"));

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<GlobalHeadersOperationFilter>();
});
builder.Services.AddTransient<GlobalExceptionMiddleware>();
builder.Services.AddTransient<CurrentUserMiddleware>();
builder.Services.AddApiVersioningSetup();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = "swagger";
        c.UseRequestInterceptor("(req) => { req.headers['CompanyId'] = req.headers['CompanyId'] || '1'; req.headers['Language'] = req.headers['Language'] || 'en'; return req; }");
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();  
app.UseAuthorization();
app.UseMiddleware<CurrentUserMiddleware>();
app.UsePermissionMiddleware();  
var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

var v1Api = app
    .MapGroup("/api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

v1Api.MapUserGroupEndpoints();
v1Api.MapFormControlEndpoints();
v1Api.MapUserEndpoints();
v1Api.MapGroupEndpoints();
v1Api.MapLoginEndpoints();
v1Api.MapMenuEndpoints();
v1Api.MapPermissionEndpoints();
v1Api.MapModuleEndpoints();
v1Api.MapModulePermissionEndpoints();
v1Api.MapCompanyEndpoints();
v1Api.MapBranchEndpoints();
v1Api.MapSectorEndpoints();
v1Api.MapDepartmentEndpoints();
v1Api.MapLocationEndpoints();
v1Api.MapPositionEndpoints();
v1Api.MapNationalityEndpoints();//24-03-2026
v1Api.MapReligionEndpoints();
v1Api.MapBloodGroupEndpoints();
v1Api.MapDependantTypeEndpoints();
v1Api.MapCurrencyEndpoints();
v1Api.MapProfessionEndpoints();
v1Api.MapEducationEndpoints();
v1Api.MapMaritalStatusEndpoints();
v1Api.MapSponsorEndpoints();//25-03-2026
v1Api.MapContractTypeEndpoints();
v1Api.MapDocumentTypesGroupEndpoints();
v1Api.MapDocumentEndpoints();
v1Api.MapSearchEndpoints();
v1Api.MapCityEndpoints();
v1Api.MapCountryEndpoints();
v1Api.MapRegionEndpoints();
v1Api.MapBankEndpoints();
v1Api.MapVacationsPaidTypeEndpoints();
v1Api.MapVacationsTypeEndpoints();
v1Api.MapGenderEndpoints();
v1Api.MapTransactionsGroupEndpoints();
v1Api.MapProjectEndpoints();
v1Api.MapIntervalEndpoints();
v1Api.MapTransactionsTypeEndpoints();
v1Api.MapEndOfServiceEndpoints();
v1Api.MapUtilsEndpoints();
v1Api.MapLookupEndpoints();
v1Api.MapItemEndpoints();
v1Api.MapHICompanyEndpoints();
v1Api.MapContractsTypeEndpoints();
v1Api.MapGradeEndpoints();
v1Api.MapGradeStepEndpoints();
v1Api.MapEmployeeClassEndpoints();
v1Api.MapFiscalYearEndpoints();
v1Api.MapFiscalYearPeriodEndpoints();


app.Run();