using Domain.UARbac;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.UARbac;


namespace Application.UARbac.Abstractions
{
    public interface IUserRepository
    {
        // Basic CRUD
        Task<Domain.UARbac.Users?> GetByIdAsync(int id);
        Task<List<Domain.UARbac.Users>> GetAllAsync();
        Task<Domain.UARbac.Users> AddAsync(Domain.UARbac.Users user);
        Task UpdateAsync(Domain.UARbac.Users user);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id );
        Task SaveChangesAsync(CancellationToken ct);

        // User specific queries
        Task<Domain.UARbac.Users?> GetByCodeAsync(string code );
        Task<List<Domain.UARbac.Users>> GetByGroupIdAsync(int groupId );
        Task<bool> IsAdminAsync(int userId );
        Task<bool> IsActiveAsync(int userId );

        // User-Group relationships
        Task<List<Domain.UARbac.UserGroup>> GetUserGroupsAsync(int userId );
        Task AddUserToGroupAsync(int userId, int groupId, bool isPrimary );
        Task RemoveUserFromGroupAsync(int userId, int groupId );
        Task UpdateUserGroupPrimaryAsync(int userId, int groupId, bool isPrimary );

        // Device token
        Task UpdateDeviceTokenAsync(int userId, string deviceToken );
    }
}