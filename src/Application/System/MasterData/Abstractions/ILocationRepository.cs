using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface ILocationRepository
    {
        // Basic CRUD
        Task<Domain.System.MasterData.Location?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Location?> GetByCodeAsync(string code, int companyId);  // ← أضف companyId إجباري
        Task<List<Domain.System.MasterData.Location>> GetAllAsync(int companyId);  // ← أضف companyId
        Task<Domain.System.MasterData.Location> AddAsync(Domain.System.MasterData.Location location);
        Task UpdateAsync(Domain.System.MasterData.Location location);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code, int companyId);
        Task<bool> CodeExistsAsync(string code, int companyId, int excludeId);

        // Specific queries
        Task<List<Domain.System.MasterData.Location>> GetByCompanyIdAsync(int companyId);
        Task<List<Domain.System.MasterData.Location>> GetByBranchIdAsync(int branchId);
        Task<List<Domain.System.MasterData.Location>> GetByDepartmentIdAsync(int departmentId);
        Task<List<Domain.System.MasterData.Location>> GetByCityIdAsync(int cityId);
        Task<List<Domain.System.MasterData.Location>> GetActiveLocationsAsync(int companyId);   
        Task<PagedResult<Domain.System.MasterData.Location>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int companyId, int? branchId = null);  // ← companyId إجباري

         Task SoftDeleteAsync(int id, int? regUserId = null);

         Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);

    }
}