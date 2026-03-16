using Application.Abstractions;
using Application.UARbac.FormPermission.Dtos;
using Domain.Common;
using Domain.System.MasterData;
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;
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
    public DbSet<BloodGroups> BloodGroups => Set<BloodGroups>();
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
   
    
    
    //Master Data

    public DbSet<Nationality> Nationalities => Set<Nationality>();






    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ✅ Loads all IEntityTypeConfiguration<> (like UserGroupConfiguration)
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
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
        var now = DateTime.Now;           // legacy DB typically uses local server time
        var userId = _currentUser.UserId; // int? (legacy)

        foreach (var entry in ChangeTracker.Entries<LegacyEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.SetRegistration(now, userId);
            }

            // Optional: protect from accidental overwrite on update
            if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(LegacyEntity.RegDate)).IsModified = false;
                entry.Property(nameof(LegacyEntity.RegUserId)).IsModified = false;
            }
        }
    }
}
