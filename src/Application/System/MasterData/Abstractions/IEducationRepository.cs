using Application.Common.Models;
using Domain.System.MasterData;

namespace Application.System.MasterData.Abstractions
{
    public interface IEducationRepository
    {
        Task<Domain.System.MasterData.Education?> GetByIdAsync(int id);
        Task<Domain.System.MasterData.Education?> GetByCodeAsync(string code  );
        Task<List<Domain.System.MasterData.Education>> GetAllAsync( );  
        Task<List<Domain.System.MasterData.Education>> GetByCompanyIdAsync();
        Task<Domain.System.MasterData.Education> AddAsync(Domain.System.MasterData.Education education);
        Task UpdateAsync(Domain.System.MasterData.Education education);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.MasterData.Education>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm );   
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

    }
}