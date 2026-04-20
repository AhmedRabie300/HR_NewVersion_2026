using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IProfessionRepository
    {
        Task<Domain.System.MasterData.Profession?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Profession?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.Profession>> GetAllAsync();   
        Task<List<Domain.System.MasterData.Profession>> GetByCompanyIdAsync();
        Task<Domain.System.MasterData.Profession> AddAsync(Domain.System.MasterData.Profession profession);
        Task UpdateAsync(Domain.System.MasterData.Profession profession);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.MasterData.Profession>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);  // ← companyId إجباري
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

    }
}