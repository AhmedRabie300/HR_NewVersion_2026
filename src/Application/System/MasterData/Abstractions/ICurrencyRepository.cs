using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface ICurrencyRepository
    {
        Task<Domain.System.MasterData.Currency?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Currency?> GetByCodeAsync(string code );
        Task<List<Domain.System.MasterData.Currency>> GetAllAsync();
        Task<List<Domain.System.MasterData.Currency>> GetByCompanyIdAsync();
        Task<Domain.System.MasterData.Currency> AddAsync(Domain.System.MasterData.Currency currency);
        Task UpdateAsync(Domain.System.MasterData.Currency currency);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code,  int excludeId);
        Task<PagedResult<Domain.System.MasterData.Currency>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

    }
}