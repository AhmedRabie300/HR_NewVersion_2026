using Application.Common.Models;
using Domain.System.HRS;

namespace Application.System.HRS.Abstractions
{
    public interface IGenderRepository
    {
        Task<Domain.System.HRS.Gender?> GetByIdAsync(int id);
        Task<Domain.System.HRS.Gender?> GetByCodeAsync(string code);
        Task<List<Domain.System.HRS.Gender>> GetAllAsync();
        Task<Domain.System.HRS.Gender> AddAsync(Domain.System.HRS.Gender entity);
        Task UpdateAsync(Domain.System.HRS.Gender entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.HRS.Gender>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);
    }
}