using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class DocumentTypesGroupRepository : IDocumentTypesGroupRepository
    {
        private readonly ApplicationDbContext _db;

        public DocumentTypesGroupRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<DocumentTypesGroup?> GetByIdAsync(int id)
            => await _db.DocumentTypesGroups.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<DocumentTypesGroup?> GetByCodeAsync(string code)
            => await _db.DocumentTypesGroups.FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<DocumentTypesGroup>> GetAllAsync()
            => await _db.DocumentTypesGroups
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<DocumentTypesGroup> AddAsync(DocumentTypesGroup entity)
        {
            await _db.DocumentTypesGroups.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(DocumentTypesGroup entity)
        {
            _db.DocumentTypesGroups.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.DocumentTypesGroups.FindAsync(id);
            if (item != null) _db.DocumentTypesGroups.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.DocumentTypesGroups.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
            => await _db.DocumentTypesGroups.AnyAsync(x => x.Code == code);

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
            => await _db.DocumentTypesGroups.AnyAsync(x => x.Code == code && x.Id != excludeId);

        public async Task<PagedResult<DocumentTypesGroup>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<DocumentTypesGroup> query = _db.DocumentTypesGroups
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

            return new PagedResult<DocumentTypesGroup>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.DocumentTypesGroups.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.DocumentTypesGroups.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            return await _db.DocumentTypesGroups
                .Where(x => x.CancelDate == null)
                .OrderByDescending(x => x.Code)
                .Select(x => x.Code)
                .FirstOrDefaultAsync(ct);
        }
    }
}