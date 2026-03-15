using Domain.UARbac;
using Application.UARbac.FormPermission.Dtos;

namespace Application.UARbac.Abstractions
{
    public interface IFormPermissionRepository
    {
        // Basic CRUD
        Task<Domain.UARbac.FormPermission?> GetByIdAsync(int id);
        Task<List<Domain.UARbac.FormPermission>> GetAllAsync();
        Task<Domain.UARbac.FormPermission> AddAsync(Domain.UARbac.FormPermission permission);
        Task UpdateAsync(Domain.UARbac.FormPermission permission);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync(CancellationToken ct);

        // Permission queries
        Task<List<Domain.UARbac.FormPermission>> GetByFormIdAsync(int formId);
        Task<List<Domain.UARbac.FormPermission>> GetByGroupIdAsync(int groupId);
        Task<List<Domain.UARbac.FormPermission>> GetByUserIdAsync(int userId);
        Task<Domain.UARbac.FormPermission?> GetByFormAndGroupAsync(int formId, int groupId);
        Task<Domain.UARbac.FormPermission?> GetByFormAndUserAsync(int formId, int userId);

        // User permissions (combined from user and groups)
        Task<List<Domain.UARbac.FormPermission>> GetUserEffectivePermissionsAsync(int userId);
        Task<Dictionary<int, bool>> GetUserFormPermissionFlagsAsync(int userId, int formId);

        // Bulk operations
        Task AddRangeAsync(IEnumerable<Domain.UARbac.FormPermission> permissions);
        Task DeleteByFormIdAsync(int formId);
        Task DeleteByGroupIdAsync(int groupId);
        Task DeleteByUserIdAsync(int userId);
        Task<bool> HasPermissionAsync(int userId, int formId, string permissionType);
    }
}