// Infrastructure/Data/Repositories/UARbac/FormPermissionRepository.cs
using Application.UARbac.Abstractions;
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.UARbac
{
    public sealed class FormPermissionRepository : IFormPermissionRepository
    {
        private readonly ApplicationDbContext _db;

        public FormPermissionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // Basic CRUD
        public async Task<FormPermission?> GetByIdAsync(int id)
        {
            return await _db.FormPermissions
               // .Include(x => x.Form)
                .Include(x => x.Group)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<FormPermission>> GetAllAsync()
        {
            return await _db.FormPermissions
                .Include(x => x.Form)
                .Include(x => x.Group)
                .Include(x => x.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<FormPermission> AddAsync(FormPermission permission)
        {
            await _db.FormPermissions.AddAsync(permission);
            return permission;
        }

        public Task UpdateAsync(FormPermission permission)
        {
            _db.FormPermissions.Update(permission);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var permission = await _db.FormPermissions.FindAsync(id);
            if (permission != null)
            {
                _db.FormPermissions.Remove(permission);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.FormPermissions.AnyAsync(x => x.Id == id);
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }

        // Permission queries
        public async Task<List<FormPermission>> GetByFormIdAsync(int formId)
        {
            return await _db.FormPermissions
                .Include(x => x.Group)
                .Include(x => x.User)
                .Where(x => x.FormId == formId && x.CancelDate == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<FormPermission>> GetByGroupIdAsync(int groupId)
        {
            return await _db.FormPermissions
                //.Include(x => x.Form)
                .Where(x => x.GroupId == groupId && x.CancelDate == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<FormPermission>> GetByUserIdAsync(int userId)
        {
            return await _db.FormPermissions
             //   .Include(x => x.Form)
                .Where(x => x.UserId == userId && x.CancelDate == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<FormPermission?> GetByFormAndGroupAsync(int formId, int groupId)
        {
            return await _db.FormPermissions
                .FirstOrDefaultAsync(x => x.FormId == formId && x.GroupId == groupId && x.CancelDate == null);
        }

        public async Task<FormPermission?> GetByFormAndUserAsync(int formId, int userId)
        {
            return await _db.FormPermissions
                .FirstOrDefaultAsync(x => x.FormId == formId && x.UserId == userId && x.CancelDate == null);
        }

        // User permissions (combined from user and groups)
        public async Task<List<FormPermission>> GetUserEffectivePermissionsAsync(int userId)
        {
             var userGroupIds = await _db.UserGroups
                .Where(x => x.UserId == userId)
                .Select(x => x.GroupId)
                .ToListAsync();

             var permissions = await _db.FormPermissions
               .Include(x => x.Form)
                .Where(x => x.CancelDate == null &&
                           (x.UserId == userId || 
                            (x.GroupId.HasValue && userGroupIds.Contains(x.GroupId.Value))))
                .AsNoTracking()
                .ToListAsync();

            return permissions;
        }

        public async Task<Dictionary<int, bool>> GetUserFormPermissionFlagsAsync(int userId, int formId)
        {
            var permissions = await GetUserEffectivePermissionsAsync(userId);
            var formPermissions = permissions.Where(x => x.FormId == formId).ToList();

            var result = new Dictionary<int, bool>
            {
                { 1, formPermissions.Any(x => x.AllowView == true) },   // View
                { 2, formPermissions.Any(x => x.AllowAdd == true) },    // Add
                { 3, formPermissions.Any(x => x.AllowEdit == true) },   // Edit
                { 4, formPermissions.Any(x => x.AllowDelete == true) }, // Delete
                { 5, formPermissions.Any(x => x.AllowPrint == true) }   // Print
            };

            return result;
        }

        // Bulk operations
        public async Task AddRangeAsync(IEnumerable<FormPermission> permissions)
        {
            await _db.FormPermissions.AddRangeAsync(permissions);
        }

        public async Task DeleteByFormIdAsync(int formId)
        {
            var permissions = await _db.FormPermissions
                .Where(x => x.FormId == formId)
                .ToListAsync();

            _db.FormPermissions.RemoveRange(permissions);
        }

        public async Task DeleteByGroupIdAsync(int groupId)
        {
            var permissions = await _db.FormPermissions
                .Where(x => x.GroupId == groupId)
                .ToListAsync();

            _db.FormPermissions.RemoveRange(permissions);
        }

        public async Task DeleteByUserIdAsync(int userId)
        {
            var permissions = await _db.FormPermissions
                .Where(x => x.UserId == userId)
                .ToListAsync();

            _db.FormPermissions.RemoveRange(permissions);
        }

        public async Task<bool> HasPermissionAsync(int userId, int formId, string permissionType)
        {
            var permissions = await GetUserEffectivePermissionsAsync(userId);
            var formPerm = permissions.FirstOrDefault(x => x.FormId == formId);

            if (formPerm == null) return false;

            return permissionType.ToLower() switch
            {
                "view" => formPerm.AllowView == true,
                "add" => formPerm.AllowAdd == true,
                "edit" => formPerm.AllowEdit == true,
                "delete" => formPerm.AllowDelete == true,
                "print" => formPerm.AllowPrint == true,
                _ => false
            };
        }
    }
}