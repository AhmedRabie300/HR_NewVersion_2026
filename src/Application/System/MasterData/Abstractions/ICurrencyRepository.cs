using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface ICurrencyRepository
    {
        Task<Domain.System.MasterData.Currency?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Currency?> GetByCodeAsync(string code, int companyId);
        Task<List<Domain.System.MasterData.Currency>> GetAllAsync(int companyId);
        Task<List<Domain.System.MasterData.Currency>> GetByCompanyIdAsync(int companyId);
        Task<Domain.System.MasterData.Currency> AddAsync(Domain.System.MasterData.Currency currency);
        Task UpdateAsync(Domain.System.MasterData.Currency currency);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code, int companyId);
        Task<bool> CodeExistsAsync(string code, int companyId, int excludeId);
        Task<PagedResult<Domain.System.MasterData.Currency>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int companyId);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
    }
}