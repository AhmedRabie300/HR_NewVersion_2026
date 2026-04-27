using Application.Common.Models;
using Domain.System.HRS.Basics.FiscalPeriod;

namespace Application.System.HRS.Abstractions
{
    public interface IFiscalYearPeriodRepository
    {
        // ==================== FiscalYearPeriod (Detail 1) ====================
        Task<FiscalYearPeriod?> GetByIdAsync(int id);
        Task<FiscalYearPeriod?> GetByCodeAsync(string code);
        Task<List<FiscalYearPeriod>> GetAllAsync();
        Task<List<FiscalYearPeriod>> GetByFiscalYearIdAsync(int fiscalYearId);
        Task<List<FiscalYearPeriod>> GetByCompanyIdAsync();
        Task<FiscalYearPeriod> AddAsync(FiscalYearPeriod entity);
        Task UpdateAsync(FiscalYearPeriod entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<FiscalYearPeriod>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task DeleteByFiscalYearIdAsync(int fiscalYearId, CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);
        Task<List<FiscalYearPeriod>> GeneratePeriodsAsync(int fiscalYearId, bool isFormal, int? sourceFiscalYearId = null, CancellationToken ct = default);

        // ==================== FiscalYearPeriodModule (Detail 2) ====================
        Task<FiscalYearPeriodModule?> GetModuleByIdAsync(int id);
        Task<List<FiscalYearPeriodModule>> GetModulesByPeriodIdAsync(int fiscalYearPeriodId);
        Task<FiscalYearPeriodModule> AddModuleAsync(FiscalYearPeriodModule entity);
        Task UpdateModuleAsync(FiscalYearPeriodModule entity);
        Task DeleteModuleAsync(int id);
        Task DeleteModulesByPeriodIdAsync(int fiscalYearPeriodId, CancellationToken ct);
        Task<bool> ModuleExistsAsync(int id);
        Task SoftDeleteModuleAsync(int id, int? regUserId = null);
        Task<Dictionary<int, List<int>>> GetOpenModulesByFiscalYearIdAsync(int fiscalYearId, CancellationToken ct);
    }
}