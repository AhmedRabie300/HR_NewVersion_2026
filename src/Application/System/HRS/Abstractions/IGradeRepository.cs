using Application.Common.Models;
using Domain.System.HRS.Basics.GradesAndClasses;

namespace Application.System.HRS.Abstractions
{
    public interface IGradeRepository
    {
        // ==================== Grade (Master) ====================
        Task<Grade?> GetByIdAsync(int id);
        Task<Grade?> GetByCodeAsync(string code);
        Task<List<Grade>> GetAllAsync();
        Task<List<Grade>> GetByCompanyIdAsync();
        Task<Grade> AddAsync(Grade entity);
        Task UpdateAsync(Grade entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<Grade>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

        // ==================== GradeTransaction (Detail) ====================
        Task<GradeTransaction?> GetTransactionByIdAsync(int id);
        Task<List<GradeTransaction>> GetTransactionsByGradeIdAsync(int gradeId);
        Task<GradeTransaction> AddTransactionAsync(GradeTransaction entity);
        Task UpdateTransactionAsync(GradeTransaction entity);
        Task DeleteTransactionAsync(int id);
        Task<bool> TransactionExistsAsync(int id);
        Task SoftDeleteTransactionAsync(int id, int? regUserId = null);
    }
}