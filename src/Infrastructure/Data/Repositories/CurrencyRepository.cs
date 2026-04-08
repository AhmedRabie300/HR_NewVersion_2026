// Infrastructure/Data/Repositories/System/MasterData/CurrencyRepository.cs
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class CurrencyRepository : ICurrencyRepository
    {
        private readonly ApplicationDbContext _db;

        public CurrencyRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Currency?> GetByIdAsync(int id)
            => await _db.Currencies
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Currency?> GetByCodeAsync(string code, int companyId)
            => await _db.Currencies
                .FirstOrDefaultAsync(x => x.Code == code && x.CompanyId == companyId);

        public async Task<List<Currency>> GetAllAsync(int companyId)
            => await _db.Currencies
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Currency>> GetByCompanyIdAsync(int companyId)
            => await _db.Currencies
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Currency> AddAsync(Currency entity)
        {
            await _db.Currencies.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Currency entity)
        {
            _db.Currencies.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Currencies.FindAsync(id);
            if (item != null) _db.Currencies.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Currencies.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code, int companyId)
            => await _db.Currencies.AnyAsync(x => x.Code == code && x.CompanyId == companyId);

        public async Task<bool> CodeExistsAsync(string code, int companyId, int excludeId)
            => await _db.Currencies.AnyAsync(x => x.Code == code && x.CompanyId == companyId && x.Id != excludeId);

        public async Task<PagedResult<Currency>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int companyId)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Currency> query = _db.Currencies
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
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

            return new PagedResult<Currency>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.Currencies.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Currencies.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}