using Application.Common.Models;

namespace Application.System.HRS.Abstractions
{
    public interface IVacationsPaidTypeRepository
    {
        Task<Domain.System.HRS.VacationAndEndOfService.VacationsPaidType?> GetByIdAsync(int id);
        Task<Domain.System.HRS.VacationAndEndOfService.VacationsPaidType?> GetByCodeAsync(string code);
        Task<List<Domain.System.HRS.VacationAndEndOfService.VacationsPaidType>> GetAllAsync();
        Task<Domain.System.HRS.VacationAndEndOfService.VacationsPaidType> AddAsync(Domain.System.HRS.VacationAndEndOfService.VacationsPaidType entity);
        Task UpdateAsync(Domain.System.HRS.VacationAndEndOfService.VacationsPaidType entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.HRS.VacationAndEndOfService.VacationsPaidType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);
    }
}