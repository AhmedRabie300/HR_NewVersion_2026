using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IEducationRepository
    {
        Task<Domain.System.MasterData.Education?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Education?> GetByCodeAsync(string code, int companyId);
        Task<List<Domain.System.MasterData.Education>> GetAllAsync(int companyId);  // ← أضف companyId
        Task<List<Domain.System.MasterData.Education>> GetByCompanyIdAsync(int companyId);
        Task<Domain.System.MasterData.Education> AddAsync(Domain.System.MasterData.Education education);
        Task UpdateAsync(Domain.System.MasterData.Education education);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code, int companyId);
        Task<bool> CodeExistsAsync(string code, int companyId, int excludeId);
        Task<PagedResult<Domain.System.MasterData.Education>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int companyId);  // ← companyId إجباري
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
    }
}