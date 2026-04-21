using Application.Common.Models;
using Domain.System.HRS.VacationAndEndOfService;

namespace Application.System.HRS.Abstractions
{
    public interface IEndOfServiceRepository
    {
         Task<EndOfService?> GetByIdAsync(int id);
        Task<EndOfService?> GetByCodeAsync(string code);
        Task<List<EndOfService>> GetAllAsync();
        Task<List<EndOfService>> GetByCompanyIdAsync(int companyId);
        Task<EndOfService> AddAsync(EndOfService entity);
        Task UpdateAsync(EndOfService entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<EndOfService>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId = null);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

        // EndOfServiceRule (Detail)
        Task<EndOfServiceRule?> GetRuleByIdAsync(int id);
        Task<List<EndOfServiceRule>> GetRulesByEndOfServiceIdAsync(int endOfServiceId);
        Task<EndOfServiceRule> AddRuleAsync(EndOfServiceRule rule);
        Task UpdateRuleAsync(EndOfServiceRule rule);
        Task DeleteRuleAsync(int id);
        Task<bool> RuleExistsAsync(int id);
        Task SoftDeleteRuleAsync(int id, int? regUserId = null);
    }
}