// Infrastructure/Data/Repositories/UARbac/GroupRepository.cs
using Application.Common.Models;
using Application.UARbac.Abstractions;
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.UARbac
{
    public sealed class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _db;

        public GroupRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // Basic CRUD
        public async Task<Group?> GetByIdAsync(int id)
        {
            return await _db.Groups
                .Include(x => x.GroupUsers)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Group>> GetAllAsync()
        {
            return await _db.Groups
                .Include(x => x.GroupUsers)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Group> AddAsync(Group group)
        {
            await _db.Groups.AddAsync(group);
            return group;
        }

        public Task UpdateAsync(Group group)
        {
            _db.Groups.Update(group);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var group = await _db.Groups.FindAsync(id);
            if (group != null)
            {
                _db.Groups.Remove(group);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Groups.AnyAsync(x => x.Id == id);
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }

        // Group specific queries
        public async Task<Group?> GetByCodeAsync(string code)
        {
            return await _db.Groups
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _db.Groups.AnyAsync(x => x.Code == code);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            return await _db.Groups.AnyAsync(x => x.Code == code && x.Id != excludeId);
        }

        public async Task<PagedResult<Group>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Group> query = _db.Groups.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(x =>
                    (x.EngName != null && x.EngName.Contains(searchTerm)) ||
                    (x.ArbName != null && x.ArbName.Contains(searchTerm)) ||
                    x.Code.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Group>(items, pageNumber, pageSize, totalCount);
        }

        // Group users queries
        public async Task<List<UserGroup>> GetGroupUsersAsync(int groupId)
        {
            return await _db.UserGroups
                .Include(x => x.User)
                .Where(x => x.GroupId == groupId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> GetUsersCountAsync(int groupId)
        {
            return await _db.UserGroups
                .Where(x => x.GroupId == groupId)
                .CountAsync();
        }

        public async Task<bool> HasUsersAsync(int groupId)
        {
            return await _db.UserGroups
                .AnyAsync(x => x.GroupId == groupId);
        }
    }
}