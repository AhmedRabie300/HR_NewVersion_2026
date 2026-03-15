// Infrastructure/Data/Repositories/UARbac/UserGroupRepository.cs
using Application.UARbac.Abstractions;
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.UARbac
{
    public sealed class UserGroupRepository : IUserGroupRepository
    {
        private readonly ApplicationDbContext _db;

        public UserGroupRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // Basic CRUD
        public async Task<UserGroup?> GetByIdAsync(int id)
        {
            return await _db.UserGroups
                .Include(x => x.User)
                .Include(x => x.Group)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<UserGroup>> GetAllAsync()
        {
            return await _db.UserGroups
                .Include(x => x.User)
                .Include(x => x.Group)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UserGroup> AddAsync(UserGroup userGroup)
        {
            await _db.UserGroups.AddAsync(userGroup);
            return userGroup;
        }

        public Task UpdateAsync(UserGroup userGroup)
        {
            _db.UserGroups.Update(userGroup);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var userGroup = await _db.UserGroups.FindAsync(id);
            if (userGroup != null)
            {
                _db.UserGroups.Remove(userGroup);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.UserGroups.AnyAsync(x => x.Id == id);
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }

        // Specific queries
        public async Task<UserGroup?> GetByUserAndGroupAsync(int userId, int groupId)
        {
            return await _db.UserGroups
                .Include(x => x.User)
                .Include(x => x.Group)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);
        }

        public async Task<List<UserGroup>> GetByUserIdAsync(int userId)
        {
            return await _db.UserGroups
                .Include(x => x.Group)
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<UserGroup>> GetByGroupIdAsync(int groupId)
        {
            return await _db.UserGroups
                .Include(x => x.User)
                .Where(x => x.GroupId == groupId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UserGroup?> GetUserPrimaryGroupAsync(int userId)
        {
            return await _db.UserGroups
                .Include(x => x.Group)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<bool> IsUserInGroupAsync(int userId, int groupId)
        {
            return await _db.UserGroups
                .AnyAsync(x => x.UserId == userId && x.GroupId == groupId);
        }

        public async Task<int> GetUserGroupsCountAsync(int userId)
        {
            return await _db.UserGroups
                .Where(x => x.UserId == userId)
                .CountAsync();
        }

        public async Task<int> GetGroupUsersCountAsync(int groupId)
        {
            return await _db.UserGroups
                .Where(x => x.GroupId == groupId)
                .CountAsync();
        }

        public async Task<List<int>> GetUserGroupIdsAsync(int userId)
        {
            return await _db.UserGroups
                .Where(x => x.UserId == userId)
                .Select(x => x.GroupId)
                .ToListAsync();
        }

        public async Task<List<int>> GetGroupUserIdsAsync(int groupId)
        {
            return await _db.UserGroups
                .Where(x => x.GroupId == groupId)
                .Select(x => x.UserId)
                .ToListAsync();
        }

        // Bulk operations
        public async Task AddRangeAsync(IEnumerable<UserGroup> userGroups)
        {
            await _db.UserGroups.AddRangeAsync(userGroups);
        }

        public async Task DeleteRangeAsync(IEnumerable<int> ids)
        {
            var userGroups = await _db.UserGroups
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            _db.UserGroups.RemoveRange(userGroups);
        }

        public async Task RemoveUserFromAllGroupsAsync(int userId)
        {
            var userGroups = await _db.UserGroups
                .Where(x => x.UserId == userId)
                .ToListAsync();

            _db.UserGroups.RemoveRange(userGroups);
        }

        public async Task RemoveAllUsersFromGroupAsync(int groupId)
        {
            var userGroups = await _db.UserGroups
                .Where(x => x.GroupId == groupId)
                .ToListAsync();

            _db.UserGroups.RemoveRange(userGroups);
        }
    }
}