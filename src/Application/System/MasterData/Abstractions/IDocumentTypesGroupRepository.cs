using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IDocumentTypesGroupRepository
    {
        Task<Domain.System.MasterData.DocumentTypesGroup?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.DocumentTypesGroup?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.DocumentTypesGroup>> GetAllAsync();
        Task<Domain.System.MasterData.DocumentTypesGroup> AddAsync(Domain.System.MasterData.DocumentTypesGroup entity);
        Task UpdateAsync(Domain.System.MasterData.DocumentTypesGroup entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.MasterData.DocumentTypesGroup>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

    }
}