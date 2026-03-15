using Application.UARbac.Abstractions;
using Application.UARbac.ModulePermissions.Dtos;
using Domain.UARbac;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.UARbac
{
    public sealed class ModulePermissionRepository : IModulePermissionRepository
    {
        private readonly ApplicationDbContext _db;

        public ModulePermissionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

         public async Task<ModulePermission?> GetByIdAsync(int id)
        {
            return await _db.ModulePermissions
                .Include(x => x.Module)
                .Include(x => x.Group)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ModulePermission>> GetAllAsync()
        {
            return await _db.ModulePermissions
                .Include(x => x.Module)
                .Include(x => x.Group)
                .Include(x => x.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ModulePermission> AddAsync(ModulePermission permission)
        {
            await _db.ModulePermissions.AddAsync(permission);
            return permission;
        }

        public Task UpdateAsync(ModulePermission permission)
        {
            _db.ModulePermissions.Update(permission);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var permission = await _db.ModulePermissions.FindAsync(id);
            if (permission != null)
                _db.ModulePermissions.Remove(permission);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.ModulePermissions.AnyAsync(x => x.Id == id);
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }

         public async Task<List<ModulePermission>> GetByModuleIdAsync(int moduleId)
        {
            return await _db.ModulePermissions
                .Include(x => x.Group)
                .Include(x => x.User)
                .Where(x => x.ModuleId == moduleId && x.CancelDate == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<ModulePermission>> GetByGroupIdAsync(int groupId)
        {
            return await _db.ModulePermissions
                .Include(x => x.Module)
                .Where(x => x.GroupId == groupId && x.CancelDate == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<ModulePermission>> GetByUserIdAsync(int userId)
        {
            return await _db.ModulePermissions
                .Include(x => x.Module)
                .Where(x => x.UserId == userId && x.CancelDate == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ModulePermission?> GetByModuleAndGroupAsync(int moduleId, int groupId)
        {
            return await _db.ModulePermissions
                .FirstOrDefaultAsync(x => x.ModuleId == moduleId &&
                                         x.GroupId == groupId &&
                                         x.CancelDate == null);
        }

        public async Task<ModulePermission?> GetByModuleAndUserAsync(int moduleId, int userId)
        {
            return await _db.ModulePermissions
                .FirstOrDefaultAsync(x => x.ModuleId == moduleId &&
                                         x.UserId == userId &&
                                         x.CancelDate == null);
        }

         public async Task<List<UserModulePermissionDto>> GetUserModulePermissionsAsync(int userId)
        {
             var userGroupIds = await _db.UserGroups
                .Where(x => x.UserId == userId)
                .Select(x => x.GroupId)
                .ToListAsync();

             var permissions = await _db.ModulePermissions
                .Include(x => x.Module)
                .Where(x => x.CancelDate == null &&
                           (x.UserId == userId ||
                            (x.GroupId.HasValue && userGroupIds.Contains(x.GroupId.Value))))
                .ToListAsync();

             var grouped = permissions
                .GroupBy(x => x.ModuleId)
                .Select(g => new UserModulePermissionDto(
                    ModuleId: g.Key,
                    ModuleCode: g.First().Module?.Code ?? "",
                    ModuleName: g.First().Module?.EngName ?? g.First().Module?.ArbName ?? "",
                    CanView: g.Any(x => x.CanView == true),
                    PermissionSource: g.Count() > 1 ? "Multiple" :
                                    (g.First().UserId.HasValue ? "User" : "Group")
                ))
                .ToList();

            return grouped;
        }

        public async Task<bool> CanUserViewModuleAsync(int userId, int moduleId)
        {
            var permissions = await GetUserModulePermissionsAsync(userId);
            return permissions.Any(x => x.ModuleId == moduleId && x.CanView);
        }

         public async Task AddRangeAsync(IEnumerable<ModulePermission> permissions)
        {
            await _db.ModulePermissions.AddRangeAsync(permissions);
        }

        public async Task DeleteByModuleIdAsync(int moduleId)
        {
            var permissions = await _db.ModulePermissions
                .Where(x => x.ModuleId == moduleId)
                .ToListAsync();
            _db.ModulePermissions.RemoveRange(permissions);
        }

        public async Task DeleteByGroupIdAsync(int groupId)
        {
            var permissions = await _db.ModulePermissions
                .Where(x => x.GroupId == groupId)
                .ToListAsync();
            _db.ModulePermissions.RemoveRange(permissions);
        }

        public async Task DeleteByUserIdAsync(int userId)
        {
            var permissions = await _db.ModulePermissions
                .Where(x => x.UserId == userId)
                .ToListAsync();
            _db.ModulePermissions.RemoveRange(permissions);
        }
    }
}