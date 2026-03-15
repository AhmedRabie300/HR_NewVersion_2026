// Application/UARbac/Abstractions/IMenuRepository.cs
using Application.Common.Models;
using Domain.UARbac;

namespace Application.UARbac.Abstractions
{
    public interface IMenuRepository
    {
        // Basic CRUD
        Task<Menu?> GetByIdAsync(int id);
        Task<List<Menu>> GetAllAsync();
        Task<Menu> AddAsync(Menu menu);
        Task UpdateAsync(Menu menu);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync(CancellationToken ct);

        // Menu specific queries
        Task<Menu?> GetByCodeAsync(string code);
        Task<List<Menu>> GetByParentIdAsync(int? parentId);
        Task<List<Menu>> GetRootMenusAsync();
        Task<List<Menu>> GetMenuHierarchyAsync();
        Task<PagedResult<Menu>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);

        // User menus (for permissions)
        Task<List<Menu>> GetUserMenusAsync(int userId);
        Task<List<Menu>> GetUserVisibleMenusAsync(int userId);

        // Form related
        Task<Menu?> GetByFormIdAsync(int formId);
        Task<List<Menu>> GetByFormIdsAsync(List<int> formIds);

        // Bulk operations
        Task UpdateRanksAsync(Dictionary<int, int> menuRanks);
    }
}