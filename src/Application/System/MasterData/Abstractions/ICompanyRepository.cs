 using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface ICompanyRepository
    {
        Task<Domain.System.MasterData.Company?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Company?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.Company>> GetAllAsync();
        Task<Domain.System.MasterData.Company> AddAsync(Domain.System.MasterData.Company company);
        Task UpdateAsync(Domain.System.MasterData.Company company);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<List<Domain.System.MasterData.Company>> GetActiveCompaniesAsync();
        Task<PagedResult<Domain.System.MasterData.Company>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
    }
}