using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface ISponsorRepository
    {
        Task<Domain.System.MasterData.Sponsor?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Sponsor?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.Sponsor>> GetAllAsync();
        Task<List<Domain.System.MasterData.Sponsor>> GetByCompanyIdAsync();
        Task<Domain.System.MasterData.Sponsor> AddAsync(Domain.System.MasterData.Sponsor sponsor);
        Task UpdateAsync(Domain.System.MasterData.Sponsor sponsor);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
         Task<PagedResult<Domain.System.MasterData.Sponsor>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, CancellationToken ct = default);

    }
}