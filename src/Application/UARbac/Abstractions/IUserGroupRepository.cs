using Domain.UARbac;

namespace Application.UARbac.Abstractions
{
    public interface IUserGroupRepository
    {
        // Basic CRUD
        Task<UserGroup?> GetByIdAsync(int id);
        Task<List<UserGroup>> GetAllAsync();
        Task<UserGroup> AddAsync(UserGroup userGroup);
        Task UpdateAsync(UserGroup userGroup);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync(CancellationToken ct);

        // Specific queries
        Task<UserGroup?> GetByUserAndGroupAsync(int userId, int groupId);
        Task<List<UserGroup>> GetByUserIdAsync(int userId);
        Task<List<UserGroup>> GetByGroupIdAsync(int groupId);
        Task<UserGroup?> GetUserPrimaryGroupAsync(int userId);
        Task<bool> IsUserInGroupAsync(int userId, int groupId);
        Task<int> GetUserGroupsCountAsync(int userId);
        Task<int> GetGroupUsersCountAsync(int groupId);
        Task<List<int>> GetUserGroupIdsAsync(int userId);
        Task<List<int>> GetGroupUserIdsAsync(int groupId);

        // Bulk operations
        Task AddRangeAsync(IEnumerable<UserGroup> userGroups);
        Task DeleteRangeAsync(IEnumerable<int> ids);
        Task RemoveUserFromAllGroupsAsync(int userId);
        Task RemoveAllUsersFromGroupAsync(int groupId);
    }
}