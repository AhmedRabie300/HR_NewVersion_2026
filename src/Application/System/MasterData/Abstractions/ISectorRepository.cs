using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface ISectorRepository
    {
        // Basic CRUD
        Task<Domain.System.MasterData.Sector?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Sector?> GetByCodeAsync(string code, int companyId);  
        Task<List<Domain.System.MasterData.Sector>> GetAllAsync(int companyId);  
        Task<Domain.System.MasterData.Sector> AddAsync(Domain.System.MasterData.Sector sector);
        Task UpdateAsync(Domain.System.MasterData.Sector sector);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code, int companyId);
        Task<bool> CodeExistsAsync(string code, int companyId, int excludeId);

        // Specific queries
        Task<List<Domain.System.MasterData.Sector>> GetByCompanyIdAsync(int companyId);
        Task<List<Domain.System.MasterData.Sector>> GetByParentIdAsync(int parentId);
        Task<List<Domain.System.MasterData.Sector>> GetActiveSectorsAsync(int companyId);  
        Task<PagedResult<Domain.System.MasterData.Sector>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int companyId);  // ← companyId إجباري

         Task SoftDeleteAsync(int id, int? regUserId = null);

         Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);

    }
}