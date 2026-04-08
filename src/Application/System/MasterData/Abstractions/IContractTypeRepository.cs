using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IContractTypeRepository
    {
        Task<Domain.System.MasterData.ContractType?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.ContractType?> GetByCodeAsync(string code, int companyId);
        Task<List<Domain.System.MasterData.ContractType>> GetAllAsync(int companyId);
        Task<List<Domain.System.MasterData.ContractType>> GetByCompanyIdAsync(int companyId);
        Task<Domain.System.MasterData.ContractType> AddAsync(Domain.System.MasterData.ContractType contractType);
        Task UpdateAsync(Domain.System.MasterData.ContractType contractType);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code, int companyId);
        Task<bool> CodeExistsAsync(string code, int companyId, int excludeId);
        Task<PagedResult<Domain.System.MasterData.ContractType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
    }
}