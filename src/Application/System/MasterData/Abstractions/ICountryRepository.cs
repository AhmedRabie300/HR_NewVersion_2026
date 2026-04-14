using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface ICountryRepository
    {
        Task<Domain.System.MasterData.Country?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Country?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.Country>> GetAllAsync();
        Task<List<Domain.System.MasterData.Country>> GetByCurrencyIdAsync(int currencyId);
        Task<List<Domain.System.MasterData.Country>> GetByNationalityIdAsync(int nationalityId);
        Task<List<Domain.System.MasterData.Country>> GetByRegionIdAsync(int regionId);
        Task<List<Domain.System.MasterData.Country>> GetMainCountriesAsync();
        Task<Domain.System.MasterData.Country> AddAsync(Domain.System.MasterData.Country country);
        Task UpdateAsync(Domain.System.MasterData.Country country);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.MasterData.Country>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
    }
}