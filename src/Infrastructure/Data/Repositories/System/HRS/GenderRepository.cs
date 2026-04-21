using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.HRS
{
    public sealed class GenderRepository : IGenderRepository
    {
        private readonly ApplicationDbContext _db;

        public GenderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Gender?> GetByIdAsync(int id)
            => await _db.Genders
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Gender?> GetByCodeAsync(string code)
            => await _db.Genders
                .FirstOrDefaultAsync(x => x.Code != null && x.Code == code);

        public async Task<List<Gender>> GetAllAsync()
            => await _db.Genders
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Gender> AddAsync(Gender entity)
        {
            await _db.Genders.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Gender entity)
        {
            _db.Genders.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Genders.FindAsync(id);
            if (item != null) _db.Genders.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Genders.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
            => await _db.Genders.AnyAsync(x => x.Code != null && x.Code == code);

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
            => await _db.Genders.AnyAsync(x => x.Code != null && x.Code == code && x.Id != excludeId);

        public async Task<PagedResult<Gender>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Gender> query = _db.Genders
                .Where(x => x.CancelDate == null)
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

            return new PagedResult<Gender>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.Genders.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Genders.Update(item);
            }
        }

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Genders
                .Where(x => x.CancelDate == null && x.Code != null && x.CompanyId==companyId)
                .Select(x => x.Code)
                .ToListAsync(ct);

            if (!allCodes.Any())
                return null;

            var maxCode = allCodes
                .Select(code => new { Code = code, Number = ExtractNumber(code!) })
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

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public async Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(engName))
                return true;

            var query = _db.Genders
                .Where(x => x.CancelDate == null
                    && x.EngName != null
                    && x.EngName.Trim().ToLower() == engName.Trim().ToLower());
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);


            return !await query.AnyAsync(ct);
        }

        public async Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(arbName))
                return true;

            var query = _db.Genders
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName.Trim() == arbName.Trim());
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);


            return !await query.AnyAsync(ct);
        }

     
    }
}