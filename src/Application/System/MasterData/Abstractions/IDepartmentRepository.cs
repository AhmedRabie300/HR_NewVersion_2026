using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IDepartmentRepository
    {
        Task<Domain.System.MasterData.Department?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Department?> GetByCodeAsync(string code);
        Task<Domain.System.MasterData.Department?> GetByCodeAsync(string code, int companyId);
        Task<List<Domain.System.MasterData.Department>> GetAllAsync();
        Task<List<Domain.System.MasterData.Department>> GetByCompanyIdAsync(int companyId);
        Task<List<Domain.System.MasterData.Department>> GetByParentIdAsync(int parentId);
        Task<Domain.System.MasterData.Department> AddAsync(Domain.System.MasterData.Department department);
        Task UpdateAsync(Domain.System.MasterData.Department department);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code, int companyId);
        Task<bool> CodeExistsAsync(string code, int companyId, int excludeId);
        Task<List<Domain.System.MasterData.Department>> GetActiveDepartmentsAsync();
        Task<PagedResult<Domain.System.MasterData.Department>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
    }
}