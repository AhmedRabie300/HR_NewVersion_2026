global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Routing;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json.Serialization;
using VenusHR.API.Endpoints;
using VenusHR.API.Endpoints.Forms;
using VenusHR.API.Endpoints.LookupEndPoints;
using VenusHR.API.Endpoints.Users;
using VenusHR.API.Helpers;
using VenusHR.Application.Common.Interfaces;
using VenusHR.Application.Common.Interfaces.Attendance;
using VenusHR.Application.Common.Interfaces.Documents;
using VenusHR.Application.Common.Interfaces.Forms;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Application.Common.Interfaces.Login;
using VenusHR.Application.Common.Interfaces.Login.Command;
using VenusHR.Application.Common.Interfaces.Menus;
using VenusHR.Application.Common.Interfaces.Permissions;
using VenusHR.Application.Common.Interfaces.SelfService;
using VenusHR.Application.Common.Interfaces.Users;
using VenusHR.Core.Master;
using VenusHR.Infrastructure;
using VenusHR.Infrastructure.Presistence.Attendance;
using VenusHR.Infrastructure.Presistence.Forms;
using VenusHR.Infrastructure.Presistence.HRServices;
using VenusHR.Infrastructure.Presistence.Login;
using VenusHR.Infrastructure.Presistence.SelfService;
using VenusHR.Infrastructure.Presistence.Users;
using VenusHR.Infrastructure.Services;
using VenusHR.Infrastructure.Services.Documents;

var builder = WebApplication.CreateBuilder(args);

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ??
    throw new ArgumentException("JWT SecretKey is not configured"));

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// ✅ API Versioning Setup
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-API-Version")
    );
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Register Services
builder.Services.AddScoped<IMaster, MasterService>();
builder.Services.AddScoped<IHRMaster, HRMasreService>();
builder.Services.AddScoped<IAnnualVacationRequestService, AnnualVacationRequestSevice>();
builder.Services.AddScoped<ILoginServices, LoginServices>();
builder.Services.AddScoped<IAttendance, AttendanceSercives>();
builder.Services.AddScoped<IDocumentsService, DocumentsService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IFormService, FormService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IFormPermissionService, FormPermissionService>();
builder.Services.AddScoped<IFormsControlService, FormsControlService>();

// Configure JWT Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Authentication & Authorization
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));
var app = builder.Build();

// Middleware Pipeline
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Custom Middleware
app.UseMiddleware<JwtMiddleware>();
app.UsePermissionMiddleware();

// Built-in Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// ✅ API Versioning Groups - دلوقتي MapGroup شغالة ✅
var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .HasApiVersion(new ApiVersion(2, 0))
    .ReportApiVersions()
    .Build();

 
var versionedGroup = app
    .MapGroup("/api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);
// Map Versioned Endpoints
versionedGroup.MapLoginEndpoints();           // /api/v1/auth/...
versionedGroup.MapSelfServiceEndpoints();      // /api/v1/self-service/...
versionedGroup.MapAttendanceEndpoints();       // /api/v1/attendance/...
versionedGroup.MapHRMasterEndpoints();         // /api/v1/hr-master/...
versionedGroup.MapDocumentsEndpoints();        // /api/v1/documents/...
versionedGroup.MapUserEndpoints();             // /api/v1/users/...
versionedGroup.MapFormsControlEndpoints();     // /api/v1/forms/...
versionedGroup.MapHRMasterLookupsEndpoints();
 app.MapLoginEndpoints();
app.MapSelfServiceEndpoints();
app.MapAttendanceEndpoints();
app.MapHRMasterEndpoints();
app.MapDocumentsEndpoints();
app.MapUserEndpoints();
app.MapFormsControlEndpoints();

app.MapControllers();

app.Run();