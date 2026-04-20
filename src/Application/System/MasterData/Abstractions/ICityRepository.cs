using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface ICityRepository
    {
        Task<Domain.System.MasterData.City> GetByIdAsync(int id);
        Task<Domain.System.MasterData.City> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.City>> GetAllAsync();
          Task<Domain.System.MasterData.City> AddAsync(Domain.System.MasterData.City city);
        Task UpdateAsync(Domain.System.MasterData.City city);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.MasterData.City>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, CancellationToken ct = default);

    }
}