using API.Common.Middleware;
using API.Common.Versioning;
using API.Endpoints;
using API.Helpers;
using API.Middleware;
using API.system.MasterData;
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
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<GlobalExceptionMiddleware>();
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
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseLanguageMiddleware();
app.UseAuthentication();  
app.UseAuthorization();   
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


app.Run();