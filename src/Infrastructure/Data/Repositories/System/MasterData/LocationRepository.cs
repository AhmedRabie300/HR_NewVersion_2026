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
        {
            return await _db.Locations
                .Include(x => x.Company)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Location?> GetByCodeAsync(string code)
        {
            return await _db.Locations
                .Include(x => x.Company)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Location?> GetByCodeAsync(string code, int? companyId)
        {
            return await _db.Locations
                .Include(x => x.Company)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.Code == code &&
                    (companyId == null || x.CompanyId == companyId));
        }

        public async Task<List<Location>> GetAllAsync()
        {
            return await _db.Locations
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Location>> GetByCompanyIdAsync(int companyId)
        {
            return await _db.Locations
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Location>> GetByBranchIdAsync(int branchId)
        {
            return await _db.Locations
                .Where(x => x.CancelDate == null && x.BranchId == branchId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Location>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _db.Locations
                .Where(x => x.CancelDate == null && x.DepartmentId == departmentId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Location>> GetByCityIdAsync(int cityId)
        {
            return await _db.Locations
                .Where(x => x.CancelDate == null && x.CityId == cityId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Location> AddAsync(Location location)
        {
            await _db.Locations.AddAsync(location);
            return location;
        }

        public Task UpdateAsync(Location location)
        {
            _db.Locations.Update(location);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var location = await _db.Locations.FindAsync(id);
            if (location != null)
            {
                _db.Locations.Remove(location);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Locations.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> CodeExistsAsync(string code, int? companyId)
        {
            return await _db.Locations
                .AnyAsync(x => x.Code == code &&
                    (companyId == null || x.CompanyId == companyId));
        }

        public async Task<bool> CodeExistsAsync(string code, int? companyId, int excludeId)
        {
            return await _db.Locations
                .AnyAsync(x => x.Code == code &&
                    (companyId == null || x.CompanyId == companyId) &&
                    x.Id != excludeId);
        }

        public async Task<List<Location>> GetActiveLocationsAsync()
        {
            return await _db.Locations
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedResult<Location>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId, int? branchId)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Location> query = _db.Locations
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .AsNoTracking();

            if (companyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == companyId.Value);
            }

            if (branchId.HasValue)
            {
                query = query.Where(x => x.BranchId == branchId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(x =>
                    (x.EngName != null && x.EngName.Contains(searchTerm)) ||
                    (x.ArbName != null && x.ArbName.Contains(searchTerm)) ||
                    x.Code.Contains(searchTerm) ||
                    (x.CostCenterCode1 != null && x.CostCenterCode1.Contains(searchTerm)) ||
                    (x.CostCenterCode2 != null && x.CostCenterCode2.Contains(searchTerm)) ||
                    (x.CostCenterCode3 != null && x.CostCenterCode3.Contains(searchTerm)) ||
                    (x.CostCenterCode4 != null && x.CostCenterCode4.Contains(searchTerm)));
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
            var location = await _db.Locations.FindAsync(id);
            if (location != null)
            {
                location.Cancel(regUserId);
                _db.Locations.Update(location);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }
    }
}