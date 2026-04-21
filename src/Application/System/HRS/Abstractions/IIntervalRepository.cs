using Application.Common.Models;
using Domain.System.HRS.Basics.FiscalTransactions;

namespace Application.System.HRS.Abstractions
{
    public interface IIntervalRepository
    {
        Task<Interval?> GetByIdAsync(int id);
        Task<Interval?> GetByCodeAsync(string code);
        Task<List<Interval>> GetAllAsync();
        Task<Interval> AddAsync(Interval interval);
        Task UpdateAsync(Interval interval);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Interval>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);

         Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);

        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);
    }
}