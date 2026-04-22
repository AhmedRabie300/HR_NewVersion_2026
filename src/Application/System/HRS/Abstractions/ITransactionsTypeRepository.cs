using Application.Common.Models;
using Domain.System.HRS.Basics.FiscalTransactions;

namespace Application.System.HRS.Abstractions
{
    public interface ITransactionsTypeRepository
    {
        Task<TransactionsType?> GetByIdAsync(int id);
        Task<TransactionsType?> GetByCodeAsync(string code);
        Task<List<TransactionsType>> GetAllAsync();
        Task<List<TransactionsType>> GetByCompanyIdAsync(int companyId);
        Task<List<TransactionsType>> GetByTransactionGroupIdAsync(int transactionGroupId);
        Task<TransactionsType> AddAsync(TransactionsType entity);
        Task UpdateAsync(TransactionsType entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<TransactionsType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId = null);
        Task SoftDeleteAsync(int id);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);
    }
}