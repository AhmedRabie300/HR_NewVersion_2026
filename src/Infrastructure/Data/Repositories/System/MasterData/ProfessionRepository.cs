// Infrastructure/Data/Repositories/System/MasterData/ProfessionRepository.cs
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class ProfessionRepository : IProfessionRepository
    {
        private readonly ApplicationDbContext _db;

        public ProfessionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Profession?> GetByIdAsync(int id)
            => await _db.Professions
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Profession?> GetByCodeAsync(string code)
            => await _db.Professions
                .FirstOrDefaultAsync(x => x.Code == code );

        public async Task<List<Profession>> GetAllAsync()
            => await _db.Professions
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Profession>> GetByCompanyIdAsync()
            => await _db.Professions
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Profession> AddAsync(Profession entity)
        {
            await _db.Professions.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Profession entity)
        {
            _db.Professions.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Professions.FindAsync(id);
            if (item != null) _db.Professions.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Professions.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
            => await _db.Professions.AnyAsync(x => x.Code == code);

        public async Task<bool> CodeExistsAsync(string code,int excludeId)
            => await _db.Professions.AnyAsync(x => x.Code == code && x.Id != excludeId);

        public async Task<PagedResult<Profession>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Profession> query = _db.Professions
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
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

            return new PagedResult<Profession>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.Professions.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Professions.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Professions
                .Where(x => x.CompanyId == companyId && x.CancelDate == null)
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

            var query = _db.Professions
                .Where(x => x.CancelDate == null
                    && x.EngName != null
                    && x.EngName.ToLower() == engName.ToLower());



            return !await query.AnyAsync(ct);
        }

        public async Task<bool> IsArbNameUniqueAsync(string arbName, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(arbName))
                return true;

            var query = _db.Professions
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName == arbName);



            return !await query.AnyAsync(ct);
        }
    }
}