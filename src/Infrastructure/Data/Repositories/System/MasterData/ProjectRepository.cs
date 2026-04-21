using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _db;

        public ProjectRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Project?> GetByIdAsync(int id)
            => await _db.Projects
                .Include(x => x.Company)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Project?> GetByCodeAsync(string code)
            => await _db.Projects.FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<Project>> GetAllAsync()
            => await _db.Projects
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .Include(x => x.Location)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Project>> GetByCompanyIdAsync()
            => await _db.Projects
                .Where(x => x.CancelDate == null )
                .Include(x => x.Company)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .Include(x => x.Location)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Project>> GetByBranchIdAsync(int branchId)
            => await _db.Projects
                .Where(x => x.CancelDate == null && x.BranchId == branchId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Project>> GetByDepartmentIdAsync(int departmentId)
            => await _db.Projects
                .Where(x => x.CancelDate == null && x.DepartmentId == departmentId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Project> AddAsync(Project entity)
        {
            await _db.Projects.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Project entity)
        {
            _db.Projects.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Projects.FindAsync(id);
            if (item != null) _db.Projects.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Projects.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
            => await _db.Projects.AnyAsync(x => x.Code == code);

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
            => await _db.Projects.AnyAsync(x => x.Code == code && x.Id != excludeId);

        public async Task<PagedResult<Project>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Project> query = _db.Projects
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.Branch)
                .Include(x => x.Department)
                .Include(x => x.Location)
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

            return new PagedResult<Project>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.Projects.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Projects.Update(item);
            }
        }

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Projects
                .Where(x => x.CompanyId == companyId && x.CancelDate == null)
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

        public async Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(engName))
                return true;

            var query = _db.Projects
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

            var query = _db.Projects
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName.Trim().ToLower() == arbName.Trim().ToLower());

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return !await query.AnyAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}