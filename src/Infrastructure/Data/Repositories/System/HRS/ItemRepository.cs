using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS.Basics;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.HRS
{
    public sealed class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _db;

        public ItemRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Item?> GetByIdAsync(int id)
            => await _db.Items
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Item?> GetByCodeAsync(string code)
            => await _db.Items
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<Item>> GetAllAsync()
            => await _db.Items
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Item>> GetByCompanyIdAsync(int companyId)
            => await _db.Items
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Item> AddAsync(Item entity)
        {
            await _db.Items.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Item entity)
        {
            _db.Items.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Items.FindAsync(id);
            if (item != null) _db.Items.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Items.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.Items.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.Items.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode && x.Id != excludeId);
        }

        public async Task<PagedResult<Item>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId = null)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Item> query = _db.Items
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .AsNoTracking();

            if (companyId.HasValue)
                query = query.Where(x => x.CompanyId == companyId.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(x =>
                    (x.EngName != null && x.EngName.Contains(searchTerm)) ||
                    (x.ArbName != null && x.ArbName.Contains(searchTerm)) ||
                    x.Code.Contains(searchTerm) ||
                    (x.LicenseNumber != null && x.LicenseNumber.Contains(searchTerm)));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(x => x.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Item>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.Items.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Items.Update(item);
            }
        }

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Items
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

            var query = _db.Items
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

            var query = _db.Items
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