using Application.Common.Models;
using Domain.System.HRS.Employees;

namespace Application.System.HRS.Abstractions
{
    public interface IEmployeeRepository
    {
        // Basic CRUD
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee?> GetByCodeAsync(string code);
        Task<Employee?> GetBySSnNoAsync(string ssnNo);
        Task<List<Employee>> GetAllAsync();
        Task<List<Employee>> GetByCompanyIdAsync();
        Task<Employee> AddAsync(Employee entity);
        Task UpdateAsync(Employee entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // Code & SSN uniqueness
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<bool> SSnNoExistsAsync(string ssnNo);
        Task<bool> SSnNoExistsAsync(string ssnNo, int excludeId);

        // Paged result
        Task<PagedResult<Employee>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);

        // Soft delete
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);

        // Get next code with prefix logic
        Task<string?> GetNextCodeAsync(int prefixType, int? referenceId, string separator, int padLength, CancellationToken ct);

        // Get manager
        Task<Employee?> GetManagerByCodeAsync(string managerCode);

         Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);
         Task<string?> GetListJsonAsync(int pageNumber, int pageSize, string? orderBy, string? orderDirection, string? criteria);
    }
}