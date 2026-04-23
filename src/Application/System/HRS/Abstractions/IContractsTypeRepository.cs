using Application.Common.Models;
using Domain.System.HRS.Basics.ContractsTypes;

namespace Application.System.HRS.Abstractions
{
    public interface IContractsTypeRepository
    {
        Task<ContractsType?> GetByIdAsync(int id);
        Task<ContractsType?> GetByCodeAsync(string code);
        Task<List<ContractsType>> GetAllAsync();
        Task<List<ContractsType>> GetByCompanyIdAsync();
        Task<ContractsType> AddAsync(ContractsType entity);
        Task UpdateAsync(ContractsType entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<ContractsType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);
    }
}