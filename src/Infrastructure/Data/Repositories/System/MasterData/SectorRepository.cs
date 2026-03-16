using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class SectorRepository : ISectorRepository
    {
        private readonly ApplicationDbContext _db;

        public SectorRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Sector?> GetByIdAsync(int id)
        {
            return await _db.Sectors
                .Include(x => x.Company)
                .Include(x => x.ParentSector)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Sector?> GetByCodeAsync(string code)
        {
            return await _db.Sectors
                .Include(x => x.Company)
                .Include(x => x.ParentSector)
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Sector?> GetByCodeAsync(string code, int companyId)
        {
            return await _db.Sectors
                .Include(x => x.Company)
                .Include(x => x.ParentSector)
                .FirstOrDefaultAsync(x => x.Code == code && x.CompanyId == companyId);
        }

        public async Task<List<Sector>> GetAllAsync()
        {
            return await _db.Sectors
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.ParentSector)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Sector>> GetByCompanyIdAsync(int companyId)
        {
            return await _db.Sectors
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.ParentSector)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Sector>> GetByParentIdAsync(int parentId)
        {
            return await _db.Sectors
                .Where(x => x.CancelDate == null && x.ParentId == parentId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Sector> AddAsync(Sector sector)
        {
            await _db.Sectors.AddAsync(sector);
            return sector;
        }

        public Task UpdateAsync(Sector sector)
        {
            _db.Sectors.Update(sector);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var sector = await _db.Sectors.FindAsync(id);
            if (sector != null)
            {
                _db.Sectors.Remove(sector);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Sectors.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> CodeExistsAsync(string code, int companyId)
        {
            return await _db.Sectors.AnyAsync(x => x.Code == code && x.CompanyId == companyId);
        }

        public async Task<bool> CodeExistsAsync(string code, int companyId, int excludeId)
        {
            return await _db.Sectors
                .AnyAsync(x => x.Code == code && x.CompanyId == companyId && x.Id != excludeId);
        }

        public async Task<List<Sector>> GetActiveSectorsAsync()
        {
            return await _db.Sectors
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedResult<Sector>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Sector> query = _db.Sectors
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.ParentSector)
                .AsNoTracking();

            if (companyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == companyId.Value);
            }

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

            return new PagedResult<Sector>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var sector = await _db.Sectors.FindAsync(id);
            if (sector != null)
            {
                sector.Cancel(regUserId);
                _db.Sectors.Update(sector);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }
    }
}