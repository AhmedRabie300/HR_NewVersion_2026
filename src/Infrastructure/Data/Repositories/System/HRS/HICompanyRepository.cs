using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS.Basics.HICompanies;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.HRS.Basics.HICompanies
{
    public sealed class HICompanyRepository : IHICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public HICompanyRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ==================== HICompany (Master) ====================

        public async Task<HICompany?> GetByIdAsync(int id)
            => await _db.HICompanies
                .Include(x => x.Company)
                .Include(x => x.Classes)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<HICompany?> GetByCodeAsync(string code)
            => await _db.HICompanies
                .Include(x => x.Company)
                .Include(x => x.Classes)
                .FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<HICompany>> GetAllAsync()
            => await _db.HICompanies
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.Classes)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<HICompany>> GetByCompanyIdAsync(int companyId)
            => await _db.HICompanies
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .Include(x => x.Classes)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<HICompany> AddAsync(HICompany entity)
        {
            await _db.HICompanies.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(HICompany entity)
        {
            _db.HICompanies.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.HICompanies.FindAsync(id);
            if (item != null) _db.HICompanies.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.HICompanies.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.HICompanies.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.HICompanies.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode && x.Id != excludeId);
        }

        public async Task<PagedResult<HICompany>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId = null)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<HICompany> query = _db.HICompanies
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.Classes)
                .AsNoTracking();

            if (companyId.HasValue)
                query = query.Where(x => x.CompanyId == companyId.Value);

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

            return new PagedResult<HICompany>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.HICompanies.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.HICompanies.Update(item);
            }
        }

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.HICompanies
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
            if (string.IsNullOrWhiteSpace(engName))
                return true;

            var trimmedEngName = engName.Trim();

            var query = _db.HICompanies
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

            var query = _db.HICompanies
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName.Trim() == trimmedArbName);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return !await query.AnyAsync(ct);
        }

        // ==================== HICompanyClass (Detail) ====================

        public async Task<HICompanyClass?> GetClassByIdAsync(int id)
            => await _db.HICompanyClasses
                .Include(x => x.HICompany)
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<HICompanyClass>> GetClassesByHICompanyIdAsync(int hiCompanyId)
            => await _db.HICompanyClasses
                .Where(x => x.HICompanyId == hiCompanyId && x.CancelDate == null)
                .OrderBy(x => x.EngName)
                .AsNoTracking()
                .ToListAsync();

        public async Task<HICompanyClass> AddClassAsync(HICompanyClass entity)
        {
            await _db.HICompanyClasses.AddAsync(entity);
            return entity;
        }

        public Task UpdateClassAsync(HICompanyClass entity)
        {
            _db.HICompanyClasses.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteClassAsync(int id)
        {
            var item = await _db.HICompanyClasses.FindAsync(id);
            if (item != null) _db.HICompanyClasses.Remove(item);
        }

        public async Task<bool> ClassExistsAsync(int id)
            => await _db.HICompanyClasses.AnyAsync(x => x.Id == id);

        public async Task SoftDeleteClassAsync(int id, int? regUserId = null)
        {
            var item = await _db.HICompanyClasses.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.HICompanyClasses.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}