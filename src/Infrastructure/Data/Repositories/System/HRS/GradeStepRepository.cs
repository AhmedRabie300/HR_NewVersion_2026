using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS.Basics.GradesAndClasses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.HRS
{
    public sealed class GradeStepRepository : IGradeStepRepository
    {
        private readonly ApplicationDbContext _db;

        public GradeStepRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ==================== GradeStep (Master) ====================

        public async Task<GradeStep?> GetByIdAsync(int id)
            => await _db.GradeSteps
                .Include(x => x.Grade)
                .Include(x => x.Company)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<GradeStep?> GetByCodeAsync(string code)
            => await _db.GradeSteps
                .Include(x => x.Grade)
                .Include(x => x.Company)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<GradeStep>> GetAllAsync()
            => await _db.GradeSteps
                .Where(x => x.CancelDate == null)
                .Include(x => x.Grade)
                .Include(x => x.Company)
                .Include(x => x.Transactions)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<GradeStep>> GetByCompanyIdAsync()
            => await _db.GradeSteps
                .Where(x => x.CancelDate == null )
                .Include(x => x.Grade)
                .Include(x => x.Company)
                .Include(x => x.Transactions)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<GradeStep>> GetByGradeIdAsync(int gradeId)
            => await _db.GradeSteps
                .Where(x => x.CancelDate == null && x.GradeId == gradeId)
                .Include(x => x.Grade)
                .Include(x => x.Company)
                .Include(x => x.Transactions)
                .OrderBy(x => x.Step)
                .AsNoTracking()
                .ToListAsync();

        public async Task<GradeStep> AddAsync(GradeStep entity)
        {
            await _db.GradeSteps.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(GradeStep entity)
        {
            _db.GradeSteps.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.GradeSteps.FindAsync(id);
            if (item != null) _db.GradeSteps.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.GradeSteps.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.GradeSteps.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.GradeSteps.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode && x.Id != excludeId);
        }

        public async Task<PagedResult<GradeStep>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm,int? gradeId = null)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<GradeStep> query = _db.GradeSteps
                .Where(x => x.CancelDate == null)
                .Include(x => x.Grade)
                .Include(x => x.Company)
                .Include(x => x.Transactions)
                .AsNoTracking();

     

            if (gradeId.HasValue)
                query = query.Where(x => x.GradeId == gradeId.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(x =>
                    (x.EngName != null && x.EngName.Contains(searchTerm)) ||
                    (x.ArbName != null && x.ArbName.Contains(searchTerm)) ||
                    x.Code.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(x => x.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<GradeStep>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.GradeSteps.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.GradeSteps.Update(item);
            }
        }

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.GradeSteps
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Select(x => x.Code)
                .ToListAsync(ct);

            if (!allCodes.Any())
                return null;

            var maxCode = allCodes
                .Select(code => new { Code = code, Number = ExtractNumberFromCode(code) })
                .Where(x => x.Number > 0)
                .OrderByDescending(x => x.Number)
                .FirstOrDefault();

            return maxCode?.Code;
        }

        private int ExtractNumberFromCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return 0;
            var match = Regex.Match(code, @"\d+$");
            if (match.Success && int.TryParse(match.Value, out int number))
                return number;
            if (int.TryParse(code, out int directNumber))
                return directNumber;
            return 0;
        }

        public async Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(engName)) return true;
            var trimmedEngName = engName.Trim();
            var query = _db.GradeSteps
                .Where(x => x.CancelDate == null && x.EngName != null && x.EngName.Trim().ToLower() == trimmedEngName.ToLower());
            if (excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync(ct);
        }

        public async Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(arbName)) return true;
            var trimmedArbName = arbName.Trim();
            var query = _db.GradeSteps
                .Where(x => x.CancelDate == null && x.ArbName != null && x.ArbName.Trim() == trimmedArbName);
            if (excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync(ct);
        }

        // ==================== GradeStepTransaction (Detail) ====================

        public async Task<GradeStepTransaction?> GetTransactionByIdAsync(int id)
            => await _db.GradeStepTransactions
                .Include(x => x.GradeStep)
                .Include(x => x.GradeTransaction)
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<GradeStepTransaction>> GetTransactionsByGradeStepIdAsync(int gradeStepId)
            => await _db.GradeStepTransactions
                .Where(x => x.GradeStepId == gradeStepId && x.CancelDate == null)
                .Include(x => x.GradeTransaction)
                .OrderBy(x => x.Id)
                .AsNoTracking()
                .ToListAsync();

        public async Task<GradeStepTransaction> AddTransactionAsync(GradeStepTransaction entity)
        {
            await _db.GradeStepTransactions.AddAsync(entity);
            return entity;
        }

        public Task UpdateTransactionAsync(GradeStepTransaction entity)
        {
            _db.GradeStepTransactions.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var item = await _db.GradeStepTransactions.FindAsync(id);
            if (item != null) _db.GradeStepTransactions.Remove(item);
        }

        public async Task<bool> TransactionExistsAsync(int id)
            => await _db.GradeStepTransactions.AnyAsync(x => x.Id == id);

        public async Task SoftDeleteTransactionAsync(int id, int? regUserId = null)
        {
            var item = await _db.GradeStepTransactions.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.GradeStepTransactions.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}