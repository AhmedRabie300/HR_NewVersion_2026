using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.HRS
{
    public sealed class VacationsPaidTypeRepository : IVacationsPaidTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public VacationsPaidTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<VacationsPaidType?> GetByIdAsync(int id)
            => await _db.VacationsPaidTypes
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<VacationsPaidType?> GetByCodeAsync(string code)
            => await _db.VacationsPaidTypes
                .FirstOrDefaultAsync(x => x.Code != null && x.Code == code);

        public async Task<List<VacationsPaidType>> GetAllAsync()
            => await _db.VacationsPaidTypes
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<VacationsPaidType> AddAsync(VacationsPaidType entity)
        {
            await _db.VacationsPaidTypes.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(VacationsPaidType entity)
        {
            _db.VacationsPaidTypes.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.VacationsPaidTypes.FindAsync(id);
            if (item != null) _db.VacationsPaidTypes.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.VacationsPaidTypes.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
            => await _db.VacationsPaidTypes.AnyAsync(x => x.Code != null && x.Code == code);

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
            => await _db.VacationsPaidTypes.AnyAsync(x => x.Code != null && x.Code == code && x.Id != excludeId);

        public async Task<PagedResult<VacationsPaidType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<VacationsPaidType> query = _db.VacationsPaidTypes
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

            return new PagedResult<VacationsPaidType>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.VacationsPaidTypes.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.VacationsPaidTypes.Update(item);
            }
        }

        public async Task<string?> GetMaxCodeAsync(CancellationToken ct)
        {
            var allCodes = await _db.VacationsPaidTypes
                .Where(x => x.CancelDate == null && x.Code != null)
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
    }
}