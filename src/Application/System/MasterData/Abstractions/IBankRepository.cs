using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IBankRepository
    {
        Task<Domain.System.MasterData.Bank?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Bank?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.Bank>> GetAllAsync();
        Task<List<Domain.System.MasterData.Bank>> GetByCompanyIdAsync(int companyId);
        Task<Domain.System.MasterData.Bank> AddAsync(Domain.System.MasterData.Bank bank);
        Task UpdateAsync(Domain.System.MasterData.Bank bank);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.MasterData.Bank>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);
    }
}