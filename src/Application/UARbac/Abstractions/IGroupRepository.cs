
using Domain.UARbac;
using Application.Common.Models;

namespace Application.UARbac.Abstractions
{
    public interface IGroupRepository
    {
        // Basic CRUD
        Task<Group?> GetByIdAsync(int id);
        Task<List<Group>> GetAllAsync();
        Task<Group> AddAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync(CancellationToken ct);

        // Group specific queries
        Task<Group?> GetByCodeAsync(string code);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Group>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);

        // Group users queries
        Task<List<UserGroup>> GetGroupUsersAsync(int groupId);
        Task<int> GetUsersCountAsync(int groupId);
        Task<bool> HasUsersAsync(int groupId);
    }
}