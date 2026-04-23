using Application.Common.Models;
using Domain.System.HRS.Basics.HICompanies;

namespace Application.System.HRS.Abstractions
{
    public interface IHICompanyRepository
    {
         Task<HICompany?> GetByIdAsync(int id);
        Task<HICompany?> GetByCodeAsync(string code);
        Task<List<HICompany>> GetAllAsync();
        Task<List<HICompany>> GetByCompanyIdAsync(int companyId);
        Task<HICompany> AddAsync(HICompany entity);
        Task UpdateAsync(HICompany entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<HICompany>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId = null);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

         Task<HICompanyClass?> GetClassByIdAsync(int id);
        Task<List<HICompanyClass>> GetClassesByHICompanyIdAsync(int hiCompanyId);
        Task<HICompanyClass> AddClassAsync(HICompanyClass entity);
        Task UpdateClassAsync(HICompanyClass entity);
        Task DeleteClassAsync(int id);
        Task<bool> ClassExistsAsync(int id);
        Task SoftDeleteClassAsync(int id, int? regUserId = null);
    }
}