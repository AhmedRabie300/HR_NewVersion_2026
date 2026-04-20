using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class SponsorRepository : ISponsorRepository
    {
        private readonly ApplicationDbContext _db;

        public SponsorRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
            => await _db.Sponsors
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Sponsor?> GetByCodeAsync(string code)
            => await _db.Sponsors.FirstOrDefaultAsync(x => x.Code == code );

        public async Task<List<Sponsor>> GetAllAsync()
        {
             
                var sponsors = await _db.Sponsors
                    .Where(x => x.CancelDate == null)
                    .Include(x => x.Company)
                    .OrderBy(x => x.Code)
                    .AsNoTracking()
                    .ToListAsync();

                return sponsors;
             
           
        }

        public async Task<List<Sponsor>> GetByCompanyIdAsync()
            => await _db.Sponsors
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Sponsor> AddAsync(Sponsor entity)
        {
            await _db.Sponsors.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Sponsor entity)
        {
            _db.Sponsors.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Sponsors.FindAsync(id);
            if (item != null) _db.Sponsors.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Sponsors.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
            => await _db.Sponsors.AnyAsync(x => x.Code == code);

         
        public async Task<PagedResult<Sponsor>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Sponsor> query = _db.Sponsors
                .Where(x => x.CancelDate == null )
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

            return new PagedResult<Sponsor>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.Sponsors.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Sponsors.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Sponsors
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

        public async Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(engName))
                return true;

            var query = _db.Sponsors
                .Where(x => x.CancelDate == null
                    && x.EngName != null
                    && x.EngName.ToLower() == engName.ToLower());

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return !await query.AnyAsync(ct);
        }

        public async Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(arbName))
                return true;

            var query = _db.Sponsors
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName == arbName);
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);


            return !await query.AnyAsync(ct);
        }
    }
}
