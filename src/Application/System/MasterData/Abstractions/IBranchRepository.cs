 using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IBranchRepository
    {
        Task<Domain.System.MasterData.Branch?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Branch?> GetByCodeAsync(string code);
        Task<Domain.System.MasterData.Branch?> GetByCodeAsync(string code, int companyId);
        Task<List<Domain.System.MasterData.Branch>> GetAllAsync();
        Task<List<Domain.System.MasterData.Branch>> GetByCompanyIdAsync(int companyId);
        Task<List<Domain.System.MasterData.Branch>> GetByParentIdAsync(int parentId);
        Task<Domain.System.MasterData.Branch> AddAsync(Domain.System.MasterData.Branch branch);
        Task UpdateAsync(Domain.System.MasterData.Branch branch);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int companyId, int excludeId);
        Task<List<Domain.System.MasterData.Branch>> GetActiveBranchesAsync();
        Task<PagedResult<Domain.System.MasterData.Branch>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

    }
}