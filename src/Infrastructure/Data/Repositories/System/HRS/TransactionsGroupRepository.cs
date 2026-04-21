using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Domain.System.HRS.Basics.FiscalTransactions;

namespace Infrastructure.Data.Repositories.System.HRS
{
    public sealed class TransactionsGroupRepository : ITransactionsGroupRepository
    {
        private readonly ApplicationDbContext _db;

        public TransactionsGroupRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup?> GetByIdAsync(int id)
            => await _db.TransactionsGroups
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup?> GetByCodeAsync(string code)
            => await _db.TransactionsGroups.FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup>> GetAllAsync()
            => await _db.TransactionsGroups
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup>> GetByCompanyIdAsync()
            => await _db.TransactionsGroups
                .Where(x => x.CancelDate == null )
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup> AddAsync(Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup entity)
        {
            await _db.TransactionsGroups.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup entity)
        {
            _db.TransactionsGroups.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.TransactionsGroups.FindAsync(id);
            if (item != null) _db.TransactionsGroups.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.TransactionsGroups.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
            => await _db.TransactionsGroups.AnyAsync(x => x.Code == code);

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
            => await _db.TransactionsGroups.AnyAsync(x => x.Code == code && x.Id != excludeId);

        public async Task<PagedResult<TransactionsGroup>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<TransactionsGroup> query = _db.TransactionsGroups
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

            return new PagedResult<TransactionsGroup>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.TransactionsGroups.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.TransactionsGroups.Update(item);
            }
        }

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.TransactionsGroups
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

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
        public async Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(engName))
                return true;

            var query = _db.TransactionsGroups
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

            var query = _db.TransactionsGroups
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName.Trim() == arbName.Trim());

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return !await query.AnyAsync(ct);
        }
    }
}