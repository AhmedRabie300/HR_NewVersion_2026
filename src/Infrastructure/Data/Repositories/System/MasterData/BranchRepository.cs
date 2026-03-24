using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext _db;

        public BranchRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Branch?> GetByIdAsync(int id)
        {
            return await _db.Branches
                .Include(x => x.Company)
                .Include(x => x.ParentBranch)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Branch?> GetByCodeAsync(string code)
        {
            return await _db.Branches
                .Include(x => x.Company)
                .Include(x => x.ParentBranch)
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Branch?> GetByCodeAsync(string code, int companyId)
        {
            return await _db.Branches
                .Include(x => x.Company)
                .Include(x => x.ParentBranch)
                .FirstOrDefaultAsync(x => x.Code == code && x.CompanyId == companyId);
        }

        public async Task<List<Branch>> GetAllAsync()
        {
            return await _db.Branches
                .Where(x => x.CancelDate == null)
                //.Include(x => x.Company)
                //.Include(x => x.ParentBranch)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Branch>> GetByCompanyIdAsync(int companyId)
        {
            return await _db.Branches
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.ParentBranch)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Branch>> GetByParentIdAsync(int parentId)
        {
            return await _db.Branches
                .Where(x => x.CancelDate == null && x.ParentId == parentId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Branch> AddAsync(Branch branch)
        {
            await _db.Branches.AddAsync(branch);
            return branch;
        }

        public Task UpdateAsync(Branch branch)
        {
            _db.Branches.Update(branch);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var branch = await _db.Branches.FindAsync(id);
            if (branch != null)
            {
                _db.Branches.Remove(branch);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Branches.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> CodeExistsAsync(string code, int companyId)
        {
            return await _db.Branches.AnyAsync(x => x.Code == code && x.CompanyId == companyId);
        }

        public async Task<bool> CodeExistsAsync(string code, int companyId, int excludeId)
        {
            return await _db.Branches
                .AnyAsync(x => x.Code == code && x.CompanyId == companyId && x.Id != excludeId);
        }

        public async Task<List<Branch>> GetActiveBranchesAsync()
        {
            return await _db.Branches
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedResult<Branch>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Branch> query = _db.Branches
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.ParentBranch)
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

            return new PagedResult<Branch>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var branch = await _db.Branches.FindAsync(id);
            if (branch != null)
            {
                branch.Cancel(regUserId);
                _db.Branches.Update(branch);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }
    }
}