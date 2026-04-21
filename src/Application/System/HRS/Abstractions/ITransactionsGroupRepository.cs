using Application.Common.Models;
using Domain.System.HRS;

namespace Application.System.HRS.Abstractions
{
    public interface ITransactionsGroupRepository
    {
        Task<Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup?> GetByIdAsync(int id);
        Task<Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup?> GetByCodeAsync(string code);
        Task<List<Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup>> GetAllAsync();
        Task<List<Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup>> GetByCompanyIdAsync();
        Task<Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup> AddAsync(Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup entity);
        Task UpdateAsync(Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);
    }
}