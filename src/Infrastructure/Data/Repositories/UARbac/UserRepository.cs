using Application.Common.Models;
using Application.UARbac.Abstractions;
using Domain.System;
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.System
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        
        public async Task<Users?> GetByIdAsync(int id)
        {
            return await _db.Users
                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Users>> GetAllAsync()
        {
            return await _db.Users
                .ToListAsync();
        }

        public async Task<Users> AddAsync(Users user)
        {
            await _db.Users.AddAsync(user);
            return user;
        }

        public Task UpdateAsync(Users user)
        {
            _db.Users.Update(user);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null)
            {
                _db.Users.Remove(user);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Users.AnyAsync(x => x.Id == id);
        }

        public Task SaveChangesAsync(CancellationToken ct)  // دي لازم تفضل عشان موجودة في الـ Interface
        {
            return _db.SaveChangesAsync(ct);
        }

        // User specific queries
        public async Task<Users?> GetByCodeAsync(string code)
        {
            return await _db.Users
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<List<Users>> GetByGroupIdAsync(int groupId)
        {
            return await _db.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsAdminAsync(int userId)
        {
            var user = await _db.Users
                .Where(x => x.Id == userId)
                .Select(x => x.IsAdmin)
                .FirstOrDefaultAsync();

            return user ?? false;
        }

        public async Task<bool> IsActiveAsync(int userId)
        {
            var user = await _db.Users
                .Where(x => x.Id == userId)
                .Select(x => x.IsActive)
                .FirstOrDefaultAsync();

            return user ?? false;
        }


        public async Task<PagedResult<Users>> ListPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Users> query = _db.Users
                .AsNoTracking();

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
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Users>(items, pageNumber, pageSize, totalCount);
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _db.Users.AnyAsync(x => x.Code == code);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            return await _db.Users.AnyAsync(x => x.Code == code && x.Id != excludeId);
        }

        public Task<List<UserGroup>> GetUserGroupsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task AddUserToGroupAsync(int userId, int groupId, bool isPrimary)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserFromGroupAsync(int userId, int groupId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserGroupPrimaryAsync(int userId, int groupId, bool isPrimary)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDeviceTokenAsync(int userId, string deviceToken)
        {
            throw new NotImplementedException();
        }
    }
}