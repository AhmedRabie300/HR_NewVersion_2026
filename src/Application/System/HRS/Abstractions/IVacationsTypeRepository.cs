using Application.Common.Models;

namespace Application.System.HRS.Abstractions
{
    public interface IVacationsTypeRepository
    {
        Task<Domain.System.HRS.VacationAndEndOfService.VacationsType?> GetByIdAsync(int id);
        Task<Domain.System.HRS.VacationAndEndOfService.VacationsType?> GetByCodeAsync(string code);
        Task<List<Domain.System.HRS.VacationAndEndOfService.VacationsType>> GetAllAsync();
        Task<List<Domain.System.HRS.VacationAndEndOfService.VacationsType>> GetByCompanyIdAsync(int companyId);
        Task<Domain.System.HRS.VacationAndEndOfService.VacationsType> AddAsync(Domain.System.HRS.VacationAndEndOfService.VacationsType entity);
        Task UpdateAsync(Domain.System.HRS.VacationAndEndOfService.VacationsType entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Domain.System.HRS.VacationAndEndOfService.VacationsType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

    }
}