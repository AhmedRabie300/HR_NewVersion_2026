using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IRegionRepository
    {
        Task<Domain.System.MasterData.Region?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Region?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.Region>> GetAllAsync();
        Task<List<Domain.System.MasterData.Region>> GetByCountryIdAsync(int countryId);
        Task<List<Domain.System.MasterData.Region>> GetByCompanyIdAsync(int companyId);
        Task<Domain.System.MasterData.Region> AddAsync(Domain.System.MasterData.Region region);
        Task UpdateAsync(Domain.System.MasterData.Region region);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.MasterData.Region>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, CancellationToken ct = default);
    }
}