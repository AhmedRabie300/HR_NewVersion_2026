using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IBloodGroupRepository
    {
        Task<Domain.System.MasterData.BloodGroup?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.BloodGroup?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.BloodGroup>> GetAllAsync();
        Task<Domain.System.MasterData.BloodGroup> AddAsync(Domain.System.MasterData.BloodGroup bloodGroup);
        Task UpdateAsync(Domain.System.MasterData.BloodGroup bloodGroup);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.MasterData.BloodGroup>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
    }
}