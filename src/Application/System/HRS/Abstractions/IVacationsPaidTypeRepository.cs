using Application.Common.Models;
using Domain.System.HRS;

namespace Application.System.HRS.Abstractions
{
    public interface IVacationsPaidTypeRepository
    {
        Task<Domain.System.HRS.VacationsPaidType?> GetByIdAsync(int id);
        Task<Domain.System.HRS.VacationsPaidType?> GetByCodeAsync(string code);
        Task<List<Domain.System.HRS.VacationsPaidType>> GetAllAsync();
        Task<Domain.System.HRS.VacationsPaidType> AddAsync(Domain.System.HRS.VacationsPaidType entity);
        Task UpdateAsync(Domain.System.HRS.VacationsPaidType entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.HRS.VacationsPaidType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(CancellationToken ct);
    }
}