using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IProfessionRepository
    {
        Task<Domain.System.MasterData.Profession?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Profession?> GetByCodeAsync(string code, int companyId);
        Task<List<Domain.System.MasterData.Profession>> GetAllAsync(int companyId);  // ← أضف companyId
        Task<List<Domain.System.MasterData.Profession>> GetByCompanyIdAsync(int companyId);
        Task<Domain.System.MasterData.Profession> AddAsync(Domain.System.MasterData.Profession profession);
        Task UpdateAsync(Domain.System.MasterData.Profession profession);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code, int companyId);
        Task<bool> CodeExistsAsync(string code, int companyId, int excludeId);
        Task<PagedResult<Domain.System.MasterData.Profession>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int companyId);  // ← companyId إجباري
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);

    }
}