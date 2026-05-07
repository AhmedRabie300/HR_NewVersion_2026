using Application.Common.Models;
using Domain.System.HRS.Basics.Contracts;

namespace Application.System.HRS.Abstractions
{
    public interface IContractRepository
    {
        // ==================== Contract (Master) ====================
        Task<Domain.System.HRS.Basics.Contracts.Contract?> GetByIdAsync(int id);
        Task<Domain.System.HRS.Basics.Contracts.Contract?> GetByNumberAsync(int number);
        Task<List<Domain.System.HRS.Basics.Contracts.Contract>> GetAllAsync();
        Task<List<Domain.System.HRS.Basics.Contracts.Contract>> GetByCompanyIdAsync();
        Task<List<Domain.System.HRS.Basics.Contracts.Contract>> GetByEmployeeIdAsync(int employeeId);
        Task<Domain.System.HRS.Basics.Contracts.Contract> AddAsync(Domain.System.HRS.Basics.Contracts.Contract entity);
        Task UpdateAsync(Domain.System.HRS.Basics.Contracts.Contract entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> NumberExistsAsync(int number);
        Task<bool> NumberExistsAsync(int number, int excludeId);
        Task<PagedResult<Domain.System.HRS.Basics.Contracts.Contract>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id);
        Task SaveChangesAsync(CancellationToken ct);
        Task<int> GetNextNumberAsync(int companyId, CancellationToken ct);

        // ==================== ContractTransaction (Detail) ====================
        Task<ContractTransaction?> GetTransactionByIdAsync(int id);
        Task<List<ContractTransaction>> GetTransactionsByContractIdAsync(int contractId);
        Task<ContractTransaction> AddTransactionAsync(ContractTransaction entity);
        Task UpdateTransactionAsync(ContractTransaction entity);
        Task DeleteTransactionAsync(int id);
        Task<bool> TransactionExistsAsync(int id);
        Task SoftDeleteTransactionAsync(int id);
        Task<string?> GetListJsonAsync(int pageNumber, int pageSize, string? orderBy, string? orderDirection, string? criteria);
    }
}