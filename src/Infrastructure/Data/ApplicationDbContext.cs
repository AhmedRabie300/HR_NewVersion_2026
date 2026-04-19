using Application.Abstractions;
using Domain.Common;
using Domain.System.HRS;
using Domain.System.MasterData;
using Domain.System.Search;
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Data;

public sealed class ApplicationDbContext : DbContext
{
    private readonly ICurrentUser _currentUser;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUser currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

     public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<Domain.UARbac.Module> Modules => Set<Domain.UARbac.Module>();
    public DbSet<ModulePermission> ModulePermissions => Set<ModulePermission>();

    public DbSet<FormControl> FormControls => Set<FormControl>();
    public DbSet<Users> Users => Set<Users>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<UserGroup> UserGroup => Set<UserGroup>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<Form> Forms => Set<Form>();
    public DbSet<FormPermission> FormPermissions => Set<FormPermission>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Sector> Sectors => Set<Sector>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<Religion> Religions => Set<Religion>();
    public DbSet<BloodGroup> BloodGroups => Set<BloodGroup>();
    public DbSet<DependantType> DependantTypes => Set<DependantType>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Profession> Professions => Set<Profession>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<MaritalStatus> MaritalStatuses => Set<MaritalStatus>();
    public DbSet<Sponsor> Sponsors => Set<Sponsor>();
    public DbSet<ContractType> ContractTypes => Set<ContractType>();
    public DbSet<DocumentTypesGroup> DocumentTypesGroups => Set<DocumentTypesGroup>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Sys_Objects> Sys_Objects => Set<Sys_Objects>();
    public DbSet<Sys_Fields> Sys_Fields => Set<Sys_Fields>();
    public DbSet<Sys_Searchs> Sys_Searchs => Set<Sys_Searchs>();
    public DbSet<Sys_SearchsColumns> Sys_SearchsColumns => Set<Sys_SearchsColumns>();
    public DbSet<Nationality> Nationalities => Set<Nationality>();

    public DbSet<City> Cities => Set<City>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Region> Regions => Set<Region>();

    public DbSet<Bank> Banks => Set<Bank>();
    //HRS
    public DbSet<VacationsPaidType> VacationsPaidTypes => Set<VacationsPaidType>();
    public DbSet<VacationsType> VacationsTypes => Set<VacationsType>();
    public DbSet<Gender> Genders => Set<Gender>();
    public DbSet<TransactionsGroup> TransactionsGroups => Set<TransactionsGroup>();








    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ✅ Loads all IEntityTypeConfiguration<> (like UserGroupConfiguration)
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ICompanyScoped).IsAssignableFrom(entityType.ClrType))
            {
                ApplyCompanyFilter(modelBuilder, entityType.ClrType);
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    private void ApplyCompanyFilter(ModelBuilder modelBuilder, Type entityType)
    {
        // Builds: entity => entity.CompanyId == _currentUser.CompanyId
        var parameter = Expression.Parameter(entityType, "e");
        var companyIdProperty = Expression.Property(parameter, nameof(ICompanyScoped.CompanyId));

        // Reference _currentUser.CompanyId dynamically so it's re-evaluated per query
        var currentUserExpr = Expression.Constant(this);
        var currentUserField = Expression.Field(currentUserExpr, nameof(_currentUser));
        var currentCompanyId = Expression.Property(currentUserField, nameof(ICurrentUser.CompanyId));

        var body = Expression.Equal(companyIdProperty, currentCompanyId);
        var lambda = Expression.Lambda(body, parameter);

        modelBuilder.Entity(entityType).HasQueryFilter(lambda);
    }
    public override int SaveChanges()
    {
        ApplyLegacyAudit();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyLegacyAudit();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyLegacyAudit()
    {
        var now = DateTime.Now;
        var userId = _currentUser.UserId;
        var companyId = _currentUser.CompanyId;

        foreach (var entry in ChangeTracker.Entries<LegacyEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.SetRegistration(now, userId);

                // Auto-set CompanyId if the entity is company-scoped
                if (entry.Entity is ICompanyScoped companyScoped && companyScoped.CompanyId == 0)
                {
                    entry.Property(nameof(ICompanyScoped.CompanyId)).CurrentValue = companyId;
                }
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(LegacyEntity.RegDate)).IsModified = false;
                entry.Property(nameof(LegacyEntity.RegUserId)).IsModified = false;

                // Prevent CompanyId from being changed on update
                if (entry.Entity is ICompanyScoped)
                {
                    entry.Property(nameof(ICompanyScoped.CompanyId)).IsModified = false;
                }
            }
        }
    }
}
