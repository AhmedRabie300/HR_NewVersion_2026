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

 var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ??
    throw new ArgumentException("JWT SecretKey is not configured"));

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

 builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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

 builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

 builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
 var app = builder.Build();

app.UseCors("AllowAll");

 if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

 app.UseHttpsRedirection();

app.UseStaticFiles();

 app.UseRouting();

app.UseMiddleware<JwtMiddleware>();

 app.UseAuthentication();
app.UseAuthorization();
app.UsePermissionMiddleware();

app.MapLoginEndpoints();
app.MapSelfServiceEndpoints();
app.MapAttendanceEndpoints();
app.MapHRMasterEndpoints();
app.MapDocumentsEndpoints();
app.MapUserEndpoints();
app.MapFormsControlEndpoints();

 app.MapControllers();

app.Run();