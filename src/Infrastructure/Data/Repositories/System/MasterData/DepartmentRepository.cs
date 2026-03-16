 using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _db;

        public DepartmentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _db.Departments
                .Include(x => x.Company)
                .Include(x => x.ParentDepartment)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Department?> GetByCodeAsync(string code)
        {
            return await _db.Departments
                .Include(x => x.Company)
                .Include(x => x.ParentDepartment)
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Department?> GetByCodeAsync(string code, int companyId)
        {
            return await _db.Departments
                .Include(x => x.Company)
                .Include(x => x.ParentDepartment)
                .FirstOrDefaultAsync(x => x.Code == code && x.CompanyId == companyId);
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _db.Departments
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.ParentDepartment)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Department>> GetByCompanyIdAsync(int companyId)
        {
            return await _db.Departments
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.ParentDepartment)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Department>> GetByParentIdAsync(int parentId)
        {
            return await _db.Departments
                .Where(x => x.CancelDate == null && x.ParentId == parentId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Department> AddAsync(Department department)
        {
            await _db.Departments.AddAsync(department);
            return department;
        }

        public Task UpdateAsync(Department department)
        {
            _db.Departments.Update(department);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _db.Departments.FindAsync(id);
            if (department != null)
            {
                _db.Departments.Remove(department);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Departments.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> CodeExistsAsync(string code, int companyId)
        {
            return await _db.Departments.AnyAsync(x => x.Code == code && x.CompanyId == companyId);
        }

        public async Task<bool> CodeExistsAsync(string code, int companyId, int excludeId)
        {
            return await _db.Departments
                .AnyAsync(x => x.Code == code && x.CompanyId == companyId && x.Id != excludeId);
        }

        public async Task<List<Department>> GetActiveDepartmentsAsync()
        {
            return await _db.Departments
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedResult<Department>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Department> query = _db.Departments
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.ParentDepartment)
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
                    x.Code.Contains(searchTerm) ||
                    (x.CostCenterCode != null && x.CostCenterCode.Contains(searchTerm)));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Department>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var department = await _db.Departments.FindAsync(id);
            if (department != null)
            {
                department.Cancel(regUserId);
                _db.Departments.Update(department);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }
    }
}