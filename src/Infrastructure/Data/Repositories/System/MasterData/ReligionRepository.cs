using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class ReligionRepository : IReligionRepository
    {
        private readonly ApplicationDbContext _db;

        public ReligionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Religion?> GetByIdAsync(int id)
        {
            return await _db.Religions
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Religion?> GetByCodeAsync(string code)
        {
            return await _db.Religions
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<List<Religion>> GetAllAsync()
        {
            return await _db.Religions
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Religion>> GetActiveReligionsAsync()
        {
            return await _db.Religions
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Religion> AddAsync(Religion religion)
        {
            await _db.Religions.AddAsync(religion);
            return religion;
        }

        public Task UpdateAsync(Religion religion)
        {
            _db.Religions.Update(religion);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var religion = await _db.Religions.FindAsync(id);
            if (religion != null)
            {
                _db.Religions.Remove(religion);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Religions.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _db.Religions.AnyAsync(x => x.Code == code);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            return await _db.Religions.AnyAsync(x => x.Code == code && x.Id != excludeId);
        }

        public async Task<PagedResult<Religion>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Religion> query = _db.Religions
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

            return new PagedResult<Religion>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var religion = await _db.Religions.FindAsync(id);
            if (religion != null)
            {
                religion.Cancel(regUserId);
                _db.Religions.Update(religion);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Religions
                .Where(x => x.CancelDate == null)
                .Select(x => x.Code)
                .ToListAsync(ct);

            if (!allCodes.Any())
                return null;

            var maxCode = allCodes
               .Select(code => new
               {
                   Code = code,
                   Number = ExtractNumberFromCode(code)
               })
               .Where(x => x.Number > 0)
               .OrderByDescending(x => x.Number)
               .FirstOrDefault();

            return maxCode?.Code;
        }

        private int ExtractNumberFromCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return 0;

            var match = Regex.Match(code, @"\d+$");
            if (match.Success && int.TryParse(match.Value, out int number))
                return number;

            if (int.TryParse(code, out int directNumber))
                return directNumber;

            return 0;
        }
        public async Task<bool> IsEngNameUniqueAsync(string engName, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(engName))
                return true;

            var query = _db.Religions
                .Where(x => x.CancelDate == null
                    && x.EngName != null
                    && x.EngName.ToLower() == engName.ToLower());



            return !await query.AnyAsync(ct);
        }

        public async Task<bool> IsArbNameUniqueAsync(string arbName, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(arbName))
                return true;

            var query = _db.Religions
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName == arbName);



            return !await query.AnyAsync(ct);
        }
    }
}