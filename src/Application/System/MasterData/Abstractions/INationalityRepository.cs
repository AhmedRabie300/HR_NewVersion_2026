using Application.Common.Models;
using Domain.System.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
namespace Application.System.MasterData.Abstractions
{
    public interface INationalityRepository
    {
         Task<Domain.System.MasterData.Nationality?> GetByIdAsync(int id);
        Task<List<Domain.System.MasterData.Nationality?>> GetAllAsync();
        Task<Domain.System.MasterData.Nationality?> AddAsync(Domain.System.MasterData.Nationality nationality);
        Task UpdateAsync(Domain.System.MasterData.Nationality nationality);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync(CancellationToken ct);

         Task<Domain.System.MasterData.Nationality?> GetByCodeAsync(string code);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<List<Domain.System.MasterData.Nationality>> GetActiveNationalitiesAsync();
        Task<List<Domain.System.MasterData.Nationality?>> GetMainNationalitiesAsync();
        Task<PagedResult<Domain.System.MasterData.Nationality>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);

         Task SoftDeleteAsync(int id, int? regUserId = null);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, CancellationToken ct = default);

    }
}