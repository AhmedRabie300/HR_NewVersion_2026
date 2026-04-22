using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS.Basics.FiscalTransactions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.HRS
{
    public sealed class IntervalRepository : IIntervalRepository
    {
        private readonly ApplicationDbContext _db;

        public IntervalRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Interval?> GetByIdAsync(int id)
            => await _db.Intervals.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Interval?> GetByCodeAsync(string code)
            => await _db.Intervals.FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<Interval>> GetAllAsync()
            => await _db.Intervals
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Interval> AddAsync(Interval entity)
        {
            await _db.Intervals.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Interval entity)
        {
            _db.Intervals.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Intervals.FindAsync(id);
            if (item != null) _db.Intervals.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Intervals.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.Intervals.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.Intervals.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode && x.Id != excludeId);
        }

        public async Task<PagedResult<Interval>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Interval> query = _db.Intervals
                .Where(x => x.CancelDate == null)
                .AsNoTracking();

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

            return new PagedResult<Interval>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var item = await _db.Intervals.FindAsync(id);
            if (item != null)
            {
                _db.Intervals.Update(item);
            }
        }
        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Intervals
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)   
                .Select(x => x.Code)
                .ToListAsync(ct);

            if (!allCodes.Any())
                return null;

            var maxCode = allCodes
                .Select(code => new { Code = code, Number = ExtractNumber(code) })
                .Where(x => x.Number > 0)
                .OrderByDescending(x => x.Number)
                .FirstOrDefault();

            return maxCode?.Code;
        }

        private int ExtractNumber(string code)
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
            if (string.IsNullOrWhiteSpace(engName))
                return true;

            var trimmedEngName = engName.Trim();

            var query = _db.Intervals
                .Where(x => x.CancelDate == null
                    && x.EngName != null
                    && x.EngName.Trim().ToLower() == trimmedEngName.ToLower());

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return !await query.AnyAsync(ct);
        }

        public async Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(arbName))
                return true;

            var trimmedArbName = arbName.Trim();

            var query = _db.Intervals
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName.Trim() == trimmedArbName);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return !await query.AnyAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}