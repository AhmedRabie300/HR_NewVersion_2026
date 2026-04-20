using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IContractTypeRepository
    {
        Task<Domain.System.MasterData.ContractType?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.ContractType?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.ContractType>> GetAllAsync();
        Task<List<Domain.System.MasterData.ContractType>> GetByCompanyIdAsync();
        Task<Domain.System.MasterData.ContractType> AddAsync(Domain.System.MasterData.ContractType contractType);
        Task UpdateAsync(Domain.System.MasterData.ContractType contractType);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int companyId, int excludeId);
        Task<PagedResult<Domain.System.MasterData.ContractType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, CancellationToken ct = default);

    }
}