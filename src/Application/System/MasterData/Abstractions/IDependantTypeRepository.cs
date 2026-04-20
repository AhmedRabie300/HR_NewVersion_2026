using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IDependantTypeRepository
    {
        Task<Domain.System.MasterData.DependantType?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.DependantType?> GetByCodeAsync(string code);
        Task<List<Domain.System.MasterData.DependantType>> GetAllAsync( );  
        Task<List<Domain.System.MasterData.DependantType>> GetByCompanyIdAsync( );
        Task<Domain.System.MasterData.DependantType> AddAsync(Domain.System.MasterData.DependantType dependantType);
        Task UpdateAsync(Domain.System.MasterData.DependantType dependantType);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code,   int excludeId);
        Task<PagedResult<Domain.System.MasterData.DependantType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm );   
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, CancellationToken ct = default);

    }
}