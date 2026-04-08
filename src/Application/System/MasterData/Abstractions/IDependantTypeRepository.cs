using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IDependantTypeRepository
    {
        Task<Domain.System.MasterData.DependantType?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.DependantType?> GetByCodeAsync(string code, int companyId);
        Task<List<Domain.System.MasterData.DependantType>> GetAllAsync(int companyId);  // ← أضف companyId
        Task<List<Domain.System.MasterData.DependantType>> GetByCompanyIdAsync(int companyId);
        Task<Domain.System.MasterData.DependantType> AddAsync(Domain.System.MasterData.DependantType dependantType);
        Task UpdateAsync(Domain.System.MasterData.DependantType dependantType);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code, int companyId);
        Task<bool> CodeExistsAsync(string code, int companyId, int excludeId);
        Task<PagedResult<Domain.System.MasterData.DependantType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int companyId);  // ← companyId إجباري
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
    }
}