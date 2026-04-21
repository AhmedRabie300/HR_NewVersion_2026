using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IProjectRepository
    {
        Task<Domain.System.MasterData.Project?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Project?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.Project>> GetAllAsync();
        Task<List<Domain.System.MasterData.Project>> GetByCompanyIdAsync();
        Task<List<Domain.System.MasterData.Project>> GetByBranchIdAsync(int branchId);
        Task<List<Domain.System.MasterData.Project>> GetByDepartmentIdAsync(int departmentId);
        Task<Domain.System.MasterData.Project> AddAsync(Domain.System.MasterData.Project project);
        Task UpdateAsync(Domain.System.MasterData.Project project);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.MasterData.Project>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);
    }
}