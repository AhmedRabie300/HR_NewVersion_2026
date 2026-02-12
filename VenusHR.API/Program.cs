using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json.Serialization;
using VenusHR.API.Endpoints;
using VenusHR.API.Endpoints.LookupEndPoints;
using VenusHR.API.Helpers;
using VenusHR.Application.Common.Interfaces;
using VenusHR.Application.Common.Interfaces.Attendance;
using VenusHR.Application.Common.Interfaces.Documents;
using VenusHR.Application.Common.Interfaces.HR_Master;
using VenusHR.Application.Common.Interfaces.Login;
using VenusHR.Application.Common.Interfaces.SelfService;
using VenusHR.Core.Master;
using VenusHR.Infrastructure;
using VenusHR.Infrastructure.Presistence.Attendance;
using VenusHR.Infrastructure.Presistence.HRServices;
using VenusHR.Infrastructure.Presistence.Login;
using VenusHR.Infrastructure.Presistence.SelfService;
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

// Register Services
builder.Services.AddScoped<IMaster, MasterService>();
builder.Services.AddScoped<IHRMaster, HRMasreService>();
builder.Services.AddScoped<IAnnualVacationRequestService, AnnualVacationRequestSevice>();
builder.Services.AddScoped<ILoginServices, LoginServices>();
builder.Services.AddScoped<IAttendance, AttendanceSercives>();
builder.Services.AddScoped<IDocumentsService, DocumentsService>();

// Configure JWT Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Authentication & Authorization (Simplified - علشان عندنا Middleware خاص بنا)
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// ================== MIDDLEWARE PIPELINE ==================
// 1. CORS أولاً
app.UseCors("AllowAll");

// 2. Swagger في Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 3. HTTPS Redirection
app.UseHttpsRedirection();

// 4. Static Files لو فيه
app.UseStaticFiles();

// 5. Routing (مهم جداً)
app.UseRouting();

// 6. الـ JWT Authentication Middleware بتاعنا
app.UseMiddleware<JwtMiddleware>();

// 7. Built-in Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 8. Map Minimal API Endpoints
app.MapLoginEndpoints();
app.MapSelfServiceEndpoints();
app.MapAttendanceEndpoints();
app.MapHRMasterEndpoints();
app.MapDocumentsEndpoints();

// 9. Map Controllers
app.MapControllers();

app.Run();