using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface ILocationRepository
    {
        // Basic CRUD
        Task<Domain.System.MasterData.Location?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Location?> GetByCodeAsync(string code );   
        Task<List<Domain.System.MasterData.Location>> GetAllAsync();  
        Task<Domain.System.MasterData.Location> AddAsync(Domain.System.MasterData.Location location);
        Task UpdateAsync(Domain.System.MasterData.Location location);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);

        // Specific queries
        Task<List<Domain.System.MasterData.Location>> GetByCompanyIdAsync();
        Task<List<Domain.System.MasterData.Location>> GetByBranchIdAsync(int branchId);
        Task<List<Domain.System.MasterData.Location>> GetByDepartmentIdAsync(int departmentId);
        Task<List<Domain.System.MasterData.Location>> GetByCityIdAsync(int cityId);
        Task<List<Domain.System.MasterData.Location>> GetActiveLocationsAsync();   
        Task<PagedResult<Domain.System.MasterData.Location>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? branchId = null);   

         Task SoftDeleteAsync(int id, int? regUserId = null);

         Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

    }
}