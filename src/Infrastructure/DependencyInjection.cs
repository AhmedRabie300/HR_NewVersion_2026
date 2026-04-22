using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.Search.Abstractions;
using Application.UARbac.Abstractions;
using Infrastructure.Common.CurrentUser;
using Infrastructure.Data;
using Infrastructure.Data.Repositories.Common;
using Infrastructure.Data.Repositories.System;
using Infrastructure.Data.Repositories.System.HRS;
using Infrastructure.Data.Repositories.System.MasterData;
using Infrastructure.Data.Repositories.System.Search;
using Infrastructure.Data.Repositories.UARbac;
using Infrastructure.Repositories.UARbac;
using Infrastructure.Services;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Common;
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
            services.AddScoped<CurrentUser>();
            services.AddScoped<ICurrentUser>(sp => sp.GetRequiredService<CurrentUser>());
            services.AddScoped<ICurrentUserInitializer>(sp => sp.GetRequiredService<CurrentUser>());
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
            services.AddScoped<ISponsorRepository, SponsorRepository>();
            services.AddScoped<IContractTypeRepository, ContractTypeRepository>();
            services.AddScoped<IDocumentTypesGroupRepository, DocumentTypesGroupRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IContextService, ContextService>();
            services.AddScoped<IGeneralSearchRepository, GeneralSearchRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            services.AddScoped<ICodeGenerationService, CodeGenerationService>();

            //HRS
            services.AddScoped<IVacationsPaidTypeRepository, VacationsPaidTypeRepository>();
            services.AddScoped<IVacationsTypeRepository, VacationsTypeRepository>();
            services.AddScoped<IGenderRepository, GenderRepository>();

            services.AddScoped<ITransactionsGroupRepository, TransactionsGroupRepository>();
            services.AddScoped<IIntervalRepository, IntervalRepository>();
            services.AddScoped<ITransactionsTypeRepository, TransactionsTypeRepository>();
            services.AddScoped<IEndOfServiceRepository, EndOfServiceRepository>();
            services.AddScoped<IEndOfServiceRepository, EndOfServiceRepository>();
            services.AddScoped<ICodeGeneratorService, CodeGeneratorService>();
            services.AddScoped<ILookupService, LookupService>();

            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
