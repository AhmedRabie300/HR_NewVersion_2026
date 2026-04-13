// Infrastructure/Data/Repositories/System/MasterData/LocationRepository.cs
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Location?> GetByIdAsync(int id)
            => await _db.Locations
                .Include(x => x.Company)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Location?> GetByCodeAsync(string code, int companyId)
            => await _db.Locations
                .FirstOrDefaultAsync(x => x.Code == code && x.CompanyId == companyId);

        public async Task<List<Location>> GetAllAsync(int companyId)
            => await _db.Locations
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Location> AddAsync(Location entity)
        {
            await _db.Locations.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Location entity)
        {
            _db.Locations.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Locations.FindAsync(id);
            if (item != null) _db.Locations.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Locations.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code, int companyId)
            => await _db.Locations.AnyAsync(x => x.Code == code && x.CompanyId == companyId);

        public async Task<bool> CodeExistsAsync(string code, int companyId, int excludeId)
            => await _db.Locations.AnyAsync(x => x.Code == code && x.CompanyId == companyId && x.Id != excludeId);

        public async Task<List<Location>> GetByCompanyIdAsync(int companyId)
            => await _db.Locations
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Location>> GetByBranchIdAsync(int branchId)
            => await _db.Locations
                .Where(x => x.CancelDate == null && x.BranchId == branchId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Location>> GetByDepartmentIdAsync(int departmentId)
            => await _db.Locations
                .Where(x => x.CancelDate == null && x.DepartmentId == departmentId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Location>> GetByCityIdAsync(int cityId)
            => await _db.Locations
                .Where(x => x.CancelDate == null && x.CityId == cityId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Location>> GetActiveLocationsAsync(int companyId)
            => await _db.Locations
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<PagedResult<Location>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int companyId, int? branchId = null)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Location> query = _db.Locations
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .AsNoTracking();

            if (branchId.HasValue)
                query = query.Where(x => x.BranchId == branchId.Value);

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

            return new PagedResult<Location>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.Locations.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Locations.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            return await _db.Locations
                .Where(x => x.CompanyId == companyId && x.CancelDate == null)
                .OrderByDescending(x => x.Code)
                .Select(x => x.Code)
                .FirstOrDefaultAsync(ct);
        }
    }
}