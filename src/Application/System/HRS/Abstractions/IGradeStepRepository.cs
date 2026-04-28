using Application.Common.Models;
using Domain.System.HRS.Basics.GradesAndClasses;

namespace Application.System.HRS.Abstractions
{
    public interface IGradeStepRepository
    {
        // ==================== GradeStep (Master) ====================
        Task<GradeStep?> GetByIdAsync(int id);
        Task<GradeStep?> GetByCodeAsync(string code);
        Task<List<GradeStep>> GetAllAsync();
        Task<List<GradeStep>> GetByCompanyIdAsync();
        Task<List<GradeStep>> GetByGradeIdAsync(int gradeId);
        Task<GradeStep> AddAsync(GradeStep entity);
        Task UpdateAsync(GradeStep entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CodeExistsAsync(string code);
        Task<bool> CodeExistsAsync(string code, int excludeId);
        Task<PagedResult<GradeStep>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm,  int? gradeId = null);
        Task SoftDeleteAsync(int id, int? regUserId = null);
        Task SaveChangesAsync(CancellationToken ct);
        Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct);
        Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default);

        // ==================== GradeStepTransaction (Detail) ====================
        Task<GradeStepTransaction?> GetTransactionByIdAsync(int id);
        Task<List<GradeStepTransaction>> GetTransactionsByGradeStepIdAsync(int gradeStepId);
        Task<GradeStepTransaction> AddTransactionAsync(GradeStepTransaction entity);
        Task UpdateTransactionAsync(GradeStepTransaction entity);
        Task DeleteTransactionAsync(int id);
        Task<bool> TransactionExistsAsync(int id);
        Task SoftDeleteTransactionAsync(int id, int? regUserId = null);
    }
}