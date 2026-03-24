using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.UARbac.Abstractions;
using Infrastructure.Common.CurrentUser;
using Infrastructure.Data;
using Infrastructure.Data.Repositories.System;
using Infrastructure.Data.Repositories.System.MasterData;
using Infrastructure.Data.Repositories.UARbac;
using Infrastructure.Repositories.UARbac;
using Infrastructure.Services;
using Infrastructure.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IFormControlRepository, FormControlRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();

            services.AddScoped<IMenuRepository, MenuRepository>();  
            services.AddScoped<IFormRepository, FormRepository>();  
            services.AddScoped<IFormPermissionRepository, FormPermissionRepository>();

            services.AddScoped<IJwtService, JwtService>();  
            services.AddScoped<IEncryptionService, EncryptionService>();

            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IModulePermissionRepository, ModulePermissionRepository>();

            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddSingleton<ILocalizationService, LocalizationService>();

            //Master Data
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ISectorRepository, SectorRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<INationalityRepository, NationalityRepository>();
            services.AddScoped<IReligionRepository, ReligionRepository>();
            services.AddScoped<IBloodGroupRepository, BloodGroupRepository>();
            services.AddScoped<IDependantTypeRepository, DependantTypeRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IProfessionRepository, ProfessionRepository>();
            services.AddScoped<IEducationRepository, EducationRepository>();
            services.AddScoped<IMaritalStatusRepository, MaritalStatusRepository>();

            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
