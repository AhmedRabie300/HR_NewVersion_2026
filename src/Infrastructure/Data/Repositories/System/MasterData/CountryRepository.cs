using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _db;

        public CountryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Country?> GetByIdAsync(int id)
            => await _db.Countries
                .Include(x => x.Currency)
                .Include(x => x.Nationality)
                .Include(x => x.Region)
                .Include(x => x.Capital)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Country?> GetByCodeAsync(string code)
            => await _db.Countries.FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<Country>> GetAllAsync()
            => await _db.Countries
                .Where(x => x.CancelDate == null)
                .Include(x => x.Currency)
                .Include(x => x.Nationality)
                .Include(x => x.Region)
                .Include(x => x.Capital)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Country>> GetByCurrencyIdAsync(int currencyId)
            => await _db.Countries
                .Where(x => x.CancelDate == null && x.CurrencyId == currencyId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Country>> GetByNationalityIdAsync(int nationalityId)
            => await _db.Countries
                .Where(x => x.CancelDate == null && x.NationalityId == nationalityId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Country>> GetByRegionIdAsync(int regionId)
            => await _db.Countries
                .Where(x => x.CancelDate == null && x.RegionId == regionId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Country>> GetMainCountriesAsync()
            => await _db.Countries
                .Where(x => x.CancelDate == null && x.IsMainCountries == true)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Country> AddAsync(Country entity)
        {
            await _db.Countries.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Country entity)
        {
            _db.Countries.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Countries.FindAsync(id);
            if (item != null) _db.Countries.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Countries.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
            => await _db.Countries.AnyAsync(x => x.Code == code);

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
            => await _db.Countries.AnyAsync(x => x.Code == code && x.Id != excludeId);

        public async Task<PagedResult<Country>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Country> query = _db.Countries
                .Where(x => x.CancelDate == null)
                .Include(x => x.Currency)
                .Include(x => x.Nationality)
                .Include(x => x.Region)
                .Include(x => x.Capital)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(x =>
                    (x.EngName != null && x.EngName.Contains(searchTerm)) ||
                    (x.ArbName != null && x.ArbName.Contains(searchTerm)) ||
                    x.Code.Contains(searchTerm) ||
                    (x.ISOAlpha2 != null && x.ISOAlpha2.Contains(searchTerm)) ||
                    (x.ISOAlpha3 != null && x.ISOAlpha3.Contains(searchTerm)));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(x => x.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Country>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.Countries.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Countries.Update(item);
            }
        }

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Countries
                .Where(x => x.CancelDate == null)
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

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public async Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(engName))
                return true;

            var query = _db.Countries
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

            var query = _db.Countries
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName.Trim() == arbName.Trim());
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);


            return !await query.AnyAsync(ct);
        }
    }
}
