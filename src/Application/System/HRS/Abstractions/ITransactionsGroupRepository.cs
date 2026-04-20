using Application.Common.Models;
using Domain.System.HRS;

namespace Application.System.HRS.Abstractions
{
    public interface ITransactionsGroupRepository
    {
        Task<Domain.System.HRS.TransactionsGroup?> GetByIdAsync(int id);
        Task<Domain.System.HRS.TransactionsGroup?> GetByCodeAsync(string code);
        Task<List<Domain.System.HRS.TransactionsGroup>> GetAllAsync();
        Task<List<Domain.System.HRS.TransactionsGroup>> GetByCompanyIdAsync();
        Task<Domain.System.HRS.TransactionsGroup> AddAsync(Domain.System.HRS.TransactionsGroup entity);
        Task UpdateAsync(Domain.System.HRS.TransactionsGroup entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.HRS.TransactionsGroup>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, CancellationToken ct = default);
    }
}