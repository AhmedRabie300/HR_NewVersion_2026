using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IReligionRepository
    {
        Task<Domain.System.MasterData.Religion?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Religion?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.Religion>> GetAllAsync();
        Task<List<Domain.System.MasterData.Religion>> GetActiveReligionsAsync();
        Task<Domain.System.MasterData.Religion> AddAsync(Domain.System.MasterData.Religion religion);
        Task UpdateAsync(Domain.System.MasterData.Religion religion);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.MasterData.Religion>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
    }
}