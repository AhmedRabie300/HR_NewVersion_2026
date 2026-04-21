using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _db;

        public DocumentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Document?> GetByIdAsync(int id)
            => await _db.Documents
                .Include(x => x.DocumentTypesGroup)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Document?> GetByCodeAsync(string code)
            => await _db.Documents.FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<Document>> GetAllAsync()
            => await _db.Documents
                .Where(x => x.CancelDate == null)
                .Include(x => x.DocumentTypesGroup)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Document>> GetByGroupIdAsync(int groupId)
            => await _db.Documents
                .Where(x => x.CancelDate == null && x.DocumentTypesGroupId == groupId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Document> AddAsync(Document entity)
        {
            await _db.Documents.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Document entity)
        {
            _db.Documents.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Documents.FindAsync(id);
            if (item != null) _db.Documents.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Documents.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
            => await _db.Documents.AnyAsync(x => x.Code == code);

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
            => await _db.Documents.AnyAsync(x => x.Code == code && x.Id != excludeId);

        public async Task<PagedResult<Document>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? groupId)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Document> query = _db.Documents
                .Where(x => x.CancelDate == null)
                .Include(x => x.DocumentTypesGroup)
                .AsNoTracking();

            if (groupId.HasValue)
                query = query.Where(x => x.DocumentTypesGroupId == groupId.Value);

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

            return new PagedResult<Document>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.Documents.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Documents.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Documents
                .Where(x => x.CancelDate == null)
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

            var query = _db.Documents
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

            var query = _db.Documents
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName.Trim() == arbName.Trim());
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);


            return !await query.AnyAsync(ct);
        }
    }
}
