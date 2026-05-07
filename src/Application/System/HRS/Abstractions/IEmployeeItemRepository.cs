using Application.Common.Models;
using Domain.System.HRS.Basics.EmployeesItems;

namespace Application.System.HRS.Abstractions
{
    public interface IEmployeeItemRepository
    {
        // Basic CRUD
        Task<EmployeeItem?> GetByIdAsync(int id);
        Task<List<EmployeeItem>> GetAllAsync();
        Task<List<EmployeeItem>> GetByCompanyIdAsync();
        Task<List<EmployeeItem>> GetByEmployeeIdAsync(int employeeId);
        Task<List<EmployeeItem>> GetByItemIdAsync(int itemId);
        Task<List<EmployeeItem>> GetUnconfirmedAsync();
        Task<List<EmployeeItem>> GetUnreturnedAsync();
        Task<EmployeeItem> AddAsync(EmployeeItem entity);
        Task UpdateAsync(EmployeeItem entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // Paged result
        Task<PagedResult<EmployeeItem>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);

        // ✅ Dynamic List with Stored Procedure
        Task<string?> GetListJsonAsync(int pageNumber, int pageSize, string? orderBy, string? orderDirection, string? criteria);

        // Soft delete
        Task SoftDeleteAsync(int id);
        Task SaveChangesAsync(CancellationToken ct);

        // Confirmation
        Task ConfirmAsync(int id, int? regUserId = null);

        // Return item
        Task ReturnItemAsync(int id, DateTime? returnedDate = null, string? returningItemStatus = null, int? regUserId = null);
    }
}