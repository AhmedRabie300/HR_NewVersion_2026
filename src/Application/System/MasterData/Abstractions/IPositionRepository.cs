// Application/System/MasterData/Abstractions/IPositionRepository.cs
using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IPositionRepository
    {
        // Basic CRUD
        Task<Domain.System.MasterData.Position?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Position?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.Position>> GetAllAsync();
        Task<Domain.System.MasterData.Position> AddAsync(Domain.System.MasterData.Position position);
        Task UpdateAsync(Domain.System.MasterData.Position position);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);

        // Specific queries
        Task<List<Domain.System.MasterData.Position>> GetByParentIdAsync(int parentId);
        Task<List<Domain.System.MasterData.Position>> GetByPositionLevelIdAsync(int positionLevelId);
        Task<List<Domain.System.MasterData.Position>> GetActivePositionsAsync();
        Task<PagedResult<Domain.System.MasterData.Position>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);

        // Soft delete
        Task SoftDeleteAsync(int id, int? regUserId = null);

        // Save changes
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, CancellationToken ct = default);

    }
}