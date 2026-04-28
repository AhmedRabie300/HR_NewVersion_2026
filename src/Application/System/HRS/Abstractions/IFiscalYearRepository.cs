using Application.Common.Models;
using Domain.System.HRS.Basics.FiscalPeriod;

namespace Application.System.HRS.Abstractions
{
    public interface IFiscalYearRepository
    {
        Task<FiscalYear?> GetByIdAsync(int id);
        Task<FiscalYear?> GetByCodeAsync(string code);
        Task<List<FiscalYear>> GetAllAsync();
        Task<List<FiscalYear>> GetByCompanyIdAsync();  
        Task<FiscalYear> AddAsync(FiscalYear entity);

        Task UpdateAsync(FiscalYear entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<FiscalYear>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);   
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);   
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);
    }
}