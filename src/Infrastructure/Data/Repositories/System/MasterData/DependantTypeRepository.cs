// Infrastructure/Data/Repositories/System/MasterData/DependantTypeRepository.cs
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class DependantTypeRepository : IDependantTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public DependantTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<DependantType?> GetByIdAsync(int id)
            => await _db.DependantTypes
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<DependantType?> GetByCodeAsync(string code, int companyId)
            => await _db.DependantTypes
                .FirstOrDefaultAsync(x => x.Code == code && x.CompanyId == companyId);

        public async Task<List<DependantType>> GetAllAsync(int companyId)
            => await _db.DependantTypes
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<DependantType>> GetByCompanyIdAsync(int companyId)
            => await _db.DependantTypes
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<DependantType> AddAsync(DependantType entity)
        {
            await _db.DependantTypes.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(DependantType entity)
        {
            _db.DependantTypes.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.DependantTypes.FindAsync(id);
            if (item != null) _db.DependantTypes.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.DependantTypes.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code, int companyId)
            => await _db.DependantTypes.AnyAsync(x => x.Code == code && x.CompanyId == companyId);

        public async Task<bool> CodeExistsAsync(string code, int companyId, int excludeId)
            => await _db.DependantTypes.AnyAsync(x => x.Code == code && x.CompanyId == companyId && x.Id != excludeId);

        public async Task<PagedResult<DependantType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int companyId)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<DependantType> query = _db.DependantTypes
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

            return new PagedResult<DependantType>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.DependantTypes.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.DependantTypes.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}