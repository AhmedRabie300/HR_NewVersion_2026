using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class MaritalStatusRepository : IMaritalStatusRepository
    {
        private readonly ApplicationDbContext _db;

        public MaritalStatusRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<MaritalStatus?> GetByIdAsync(int id)
            => await _db.MaritalStatuses.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<MaritalStatus?> GetByCodeAsync(string code)
            => await _db.MaritalStatuses.FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<MaritalStatus>> GetAllAsync()
            => await _db.MaritalStatuses
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<MaritalStatus> AddAsync(MaritalStatus entity)
        {
            await _db.MaritalStatuses.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(MaritalStatus entity)
        {
            _db.MaritalStatuses.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.MaritalStatuses.FindAsync(id);
            if (item != null) _db.MaritalStatuses.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.MaritalStatuses.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
            => await _db.MaritalStatuses.AnyAsync(x => x.Code == code);

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
            => await _db.MaritalStatuses.AnyAsync(x => x.Code == code && x.Id != excludeId);

        public async Task<PagedResult<MaritalStatus>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<MaritalStatus> query = _db.MaritalStatuses
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

            return new PagedResult<MaritalStatus>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.MaritalStatuses.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.MaritalStatuses.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            return await _db.MaritalStatuses
                .Where(x => x.CancelDate == null)
                .OrderByDescending(x => x.Code)
                .Select(x => x.Code)
                .FirstOrDefaultAsync(ct);
        }
    }
}