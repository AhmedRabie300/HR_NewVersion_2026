using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IMaritalStatusRepository
    {
        Task<Domain.System.MasterData.MaritalStatus?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.MaritalStatus?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.MaritalStatus>> GetAllAsync();
        Task<Domain.System.MasterData.MaritalStatus> AddAsync(Domain.System.MasterData.MaritalStatus maritalStatus);
        Task UpdateAsync(Domain.System.MasterData.MaritalStatus maritalStatus);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.MasterData.MaritalStatus>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);

    }
}