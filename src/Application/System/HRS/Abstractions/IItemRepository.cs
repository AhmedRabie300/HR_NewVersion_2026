using Application.Common.Models;
using Domain.System.HRS.Basics;
using Domain.System.HRS.Basics;

namespace Application.System.HRS.Abstractions
{
    public interface IItemRepository
    {
        // Basic CRUD
        Task<Item?> GetByIdAsync(int id);
        Task<Item?> GetByCodeAsync(string code);
        Task<List<Item>> GetAllAsync();
        Task<List<Item>> GetByCompanyIdAsync(int companyId);
        Task<Item> AddAsync(Item entity);
        Task UpdateAsync(Item entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

         Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);

         Task<PagedResult<Item>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId = null);

         Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);

         Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);

         Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);
    }
}