using Domain.UARbac;
using Application.UARbac.ModulePermissions.Dtos;

namespace Application.UARbac.Abstractions
{
    public interface IModulePermissionRepository
    {
         Task<ModulePermission?> GetByIdAsync(int id);
        Task<List<ModulePermission>> GetAllAsync();
        Task<ModulePermission> AddAsync(ModulePermission permission);
        Task UpdateAsync(ModulePermission permission);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync(CancellationToken ct);

         Task<List<ModulePermission>> GetByModuleIdAsync(int moduleId);
        Task<List<ModulePermission>> GetByGroupIdAsync(int groupId);
        Task<List<ModulePermission>> GetByUserIdAsync(int userId);
        Task<ModulePermission?> GetByModuleAndGroupAsync(int moduleId, int groupId);
        Task<ModulePermission?> GetByModuleAndUserAsync(int moduleId, int userId);

         Task<List<UserModulePermissionDto>> GetUserModulePermissionsAsync(int userId);
        Task<bool> CanUserViewModuleAsync(int userId, int moduleId);

        // Bulk operations
        Task AddRangeAsync(IEnumerable<ModulePermission> permissions);
        Task DeleteByModuleIdAsync(int moduleId);
        Task DeleteByGroupIdAsync(int groupId);
        Task DeleteByUserIdAsync(int userId);
    }
}