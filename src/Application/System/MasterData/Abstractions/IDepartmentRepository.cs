using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IDepartmentRepository
    {
        Task<Domain.System.MasterData.Department?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Department?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.Department>> GetAllAsync();
        Task<List<Domain.System.MasterData.Department>> GetByCompanyIdAsync();
        Task<List<Domain.System.MasterData.Department>> GetByParentIdAsync(int parentId);
        Task<Domain.System.MasterData.Department> AddAsync(Domain.System.MasterData.Department department);
        Task UpdateAsync(Domain.System.MasterData.Department department);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<List<Domain.System.MasterData.Department>> GetActiveDepartmentsAsync();
        Task<PagedResult<Domain.System.MasterData.Department>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

    }
}