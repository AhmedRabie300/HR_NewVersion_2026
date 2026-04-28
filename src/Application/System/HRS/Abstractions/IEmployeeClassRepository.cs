using Application.Common.Models;
using Domain.System.HRS.Basics.GradesAndClasses;

namespace Application.System.HRS.Abstractions
{
    public interface IEmployeeClassRepository
    {
        // ==================== EmployeeClass (Master) ====================
        Task<EmployeeClass?> GetByIdAsync(int id);
        Task<EmployeeClass?> GetByCodeAsync(string code);
        Task<List<EmployeeClass>> GetAllAsync();
        Task<List<EmployeeClass>> GetByCompanyIdAsync();
        Task<EmployeeClass> AddAsync(EmployeeClass entity);
        Task UpdateAsync(EmployeeClass entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<EmployeeClass>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

        // ==================== EmployeeClassDelay (Detail 1) ====================
        Task<EmployeeClassDelay?> GetDelayByIdAsync(int id);
        Task<List<EmployeeClassDelay>> GetDelaysByClassIdAsync(int classId);
        Task<EmployeeClassDelay> AddDelayAsync(EmployeeClassDelay entity);
        Task UpdateDelayAsync(EmployeeClassDelay entity);
        Task DeleteDelayAsync(int id);
        Task<bool> DelayExistsAsync(int id);
        Task SoftDeleteDelayAsync(int id, int? regUserId = null);

        // ==================== EmployeeClassVacation (Detail 2) ====================
        Task<EmployeeClassVacation?> GetVacationByIdAsync(int id);
        Task<List<EmployeeClassVacation>> GetVacationsByClassIdAsync(int classId);
        Task<EmployeeClassVacation> AddVacationAsync(EmployeeClassVacation entity);
        Task UpdateVacationAsync(EmployeeClassVacation entity);
        Task DeleteVacationAsync(int id);
        Task<bool> VacationExistsAsync(int id);
        Task SoftDeleteVacationAsync(int id, int? regUserId = null);
    }
}