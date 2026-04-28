using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS.Basics.GradesAndClasses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.HRS
{
    public sealed class GradeRepository : IGradeRepository
    {
        private readonly ApplicationDbContext _db;

        public GradeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ==================== Grade (Master) ====================

        public async Task<Grade?> GetByIdAsync(int id)
            => await _db.Grades
                .Include(x => x.Company)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Grade?> GetByCodeAsync(string code)
            => await _db.Grades
                .Include(x => x.Company)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<Grade>> GetAllAsync()
            => await _db.Grades
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.Transactions)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Grade>> GetByCompanyIdAsync()
            => await _db.Grades
                .Where(x => x.CancelDate == null )
                .Include(x => x.Company)
                .Include(x => x.Transactions)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Grade> AddAsync(Grade entity)
        {
            await _db.Grades.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Grade entity)
        {
            _db.Grades.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Grades.FindAsync(id);
            if (item != null) _db.Grades.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Grades.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.Grades.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.Grades.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode && x.Id != excludeId);
        }

        public async Task<PagedResult<Grade>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Grade> query = _db.Grades
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.Transactions)
                .AsNoTracking();

            

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(x =>
                    (x.EngName != null && x.EngName.Contains(searchTerm)) ||
                    (x.ArbName != null && x.ArbName.Contains(searchTerm)) ||
                    (x.Code != null && x.Code.Contains(searchTerm)));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(x => x.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Grade>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.Grades.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Grades.Update(item);
            }
        }

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Grades
                .Where(x => x.CancelDate == null && x.CompanyId == companyId && x.Code != null)
                .Select(x => x.Code!)
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
            var query = _db.Grades
                .Where(x => x.CancelDate == null && x.EngName != null && x.EngName.Trim().ToLower() == trimmedEngName.ToLower());
            if (excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync(ct);
        }

        public async Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(arbName)) return true;
            var trimmedArbName = arbName.Trim();
            var query = _db.Grades
                .Where(x => x.CancelDate == null && x.ArbName != null && x.ArbName.Trim() == trimmedArbName);
            if (excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync(ct);
        }

        // ==================== GradeTransaction (Detail) ====================

        public async Task<GradeTransaction?> GetTransactionByIdAsync(int id)
            => await _db.GradeTransactions
                .Include(x => x.Grade)
                .Include(x => x.TransactionType)
                .Include(x => x.Interval)
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<GradeTransaction>> GetTransactionsByGradeIdAsync(int gradeId)
            => await _db.GradeTransactions
                .Where(x => x.GradeId == gradeId && x.CancelDate == null)
                .Include(x => x.TransactionType)
                .Include(x => x.Interval)
                .OrderBy(x => x.Id)
                .AsNoTracking()
                .ToListAsync();

        public async Task<GradeTransaction> AddTransactionAsync(GradeTransaction entity)
        {
            await _db.GradeTransactions.AddAsync(entity);
            return entity;
        }

        public Task UpdateTransactionAsync(GradeTransaction entity)
        {
            _db.GradeTransactions.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var item = await _db.GradeTransactions.FindAsync(id);
            if (item != null) _db.GradeTransactions.Remove(item);
        }

        public async Task<bool> TransactionExistsAsync(int id)
            => await _db.GradeTransactions.AnyAsync(x => x.Id == id);

        public async Task SoftDeleteTransactionAsync(int id, int? regUserId = null)
        {
            var item = await _db.GradeTransactions.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.GradeTransactions.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}