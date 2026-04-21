using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS.VacationAndEndOfService;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.HRS
{
    public sealed class EndOfServiceRepository : IEndOfServiceRepository
    {
        private readonly ApplicationDbContext _db;

        public EndOfServiceRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ==================== EndOfService (Header) ====================

        public async Task<EndOfService?> GetByIdAsync(int id)
            => await _db.EndOfServices
                .Include(x => x.Company)
                .Include(x => x.Rules)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<EndOfService?> GetByCodeAsync(string code)
            => await _db.EndOfServices
                .Include(x => x.Rules)
                .FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<EndOfService>> GetAllAsync()
            => await _db.EndOfServices
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.Rules)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<EndOfService>> GetByCompanyIdAsync(int companyId)
            => await _db.EndOfServices
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .Include(x => x.Rules)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<EndOfService> AddAsync(EndOfService entity)
        {
            await _db.EndOfServices.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(EndOfService entity)
        {
            _db.EndOfServices.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.EndOfServices.FindAsync(id);
            if (item != null) _db.EndOfServices.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.EndOfServices.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.EndOfServices.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.EndOfServices.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode && x.Id != excludeId);
        }

        public async Task<PagedResult<EndOfService>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId = null)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<EndOfService> query = _db.EndOfServices
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.Rules)
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

            return new PagedResult<EndOfService>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.EndOfServices.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.EndOfServices.Update(item);
            }
        }

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.EndOfServices
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

            var query = _db.EndOfServices
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

            var query = _db.EndOfServices
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName.Trim() == trimmedArbName);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return !await query.AnyAsync(ct);
        }

        // ==================== EndOfServiceRule (Detail) ====================

        public async Task<EndOfServiceRule?> GetRuleByIdAsync(int id)
            => await _db.EndOfServiceRules
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<EndOfServiceRule>> GetRulesByEndOfServiceIdAsync(int endOfServiceId)
            => await _db.EndOfServiceRules
                .Where(x => x.EndOfServiceId == endOfServiceId && x.CancelDate == null)
                .OrderBy(x => x.FromWorkingMonths)
                .AsNoTracking()
                .ToListAsync();

        public async Task<EndOfServiceRule> AddRuleAsync(EndOfServiceRule rule)
        {
            await _db.EndOfServiceRules.AddAsync(rule);
            return rule;
        }

        public Task UpdateRuleAsync(EndOfServiceRule rule)
        {
            _db.EndOfServiceRules.Update(rule);
            return Task.CompletedTask;
        }

        public async Task DeleteRuleAsync(int id)
        {
            var item = await _db.EndOfServiceRules.FindAsync(id);
            if (item != null) _db.EndOfServiceRules.Remove(item);
        }

        public async Task<bool> RuleExistsAsync(int id)
            => await _db.EndOfServiceRules.AnyAsync(x => x.Id == id);

        public async Task SoftDeleteRuleAsync(int id, int? regUserId = null)
        {
            var item = await _db.EndOfServiceRules.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.EndOfServiceRules.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}