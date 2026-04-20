using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class NationalityRepository : INationalityRepository
    {
        private readonly ApplicationDbContext _db;

        public NationalityRepository(ApplicationDbContext db)
        {
            _db = db;
        }

 
        public async Task<Nationality?> GetByIdAsync(int id)
        {
            return await _db.Nationalities
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Nationality>> GetAllAsync()
        {
            return await _db.Nationalities
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Nationality> AddAsync(Nationality nationality)
        {
            await _db.Nationalities.AddAsync(nationality);
            return nationality;
        }

        public Task UpdateAsync(Nationality nationality)
        {
            _db.Nationalities.Update(nationality);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var nationality = await _db.Nationalities.FindAsync(id);
            if (nationality != null)
            {
                _db.Nationalities.Remove(nationality);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Nationalities.AnyAsync(x => x.Id == id);
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }

        // ========== Nationality specific queries ==========

        public async Task<Nationality?> GetByCodeAsync(string code)
        {
            return await _db.Nationalities
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _db.Nationalities.AnyAsync(x => x.Code == code);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            return await _db.Nationalities
                .AnyAsync(x => x.Code == code && x.Id != excludeId);
        }

        public async Task<List<Nationality>> GetActiveNationalitiesAsync()
        {
            return await _db.Nationalities
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Nationality>> GetMainNationalitiesAsync()
        {
            return await _db.Nationalities
                .Where(x => x.CancelDate == null && x.IsMainNationality == true)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedResult<Nationality>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Nationality> query = _db.Nationalities
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

            return new PagedResult<Nationality>(items, pageNumber, pageSize, totalCount);
        }

        // ========== Soft Delete ==========

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var nationality = await _db.Nationalities.FindAsync(id);
            if (nationality != null)
            {
                nationality.Cancel(regUserId);
                _db.Nationalities.Update(nationality);
            }
        }


        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Nationalities
                .Where(x =>  x.CancelDate == null)
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

            var query = _db.Nationalities
                .Where(x => x.CancelDate == null
                    && x.EngName != null
                    && x.EngName.ToLower() == engName.ToLower());



            return !await query.AnyAsync(ct);
        }

        public async Task<bool> IsArbNameUniqueAsync(string arbName, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(arbName))
                return true;

            var query = _db.Nationalities
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName == arbName);



            return !await query.AnyAsync(ct);
        }
    }
}