using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IDocumentRepository
    {
        Task<Domain.System.MasterData.Document?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Document?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.Document>> GetAllAsync();
        Task<List<Domain.System.MasterData.Document>> GetByGroupIdAsync(int groupId);
        Task<Domain.System.MasterData.Document> AddAsync(Domain.System.MasterData.Document document);
        Task UpdateAsync(Domain.System.MasterData.Document document);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.MasterData.Document>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? groupId);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
    }
}