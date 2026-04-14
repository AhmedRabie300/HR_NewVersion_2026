// Infrastructure/Data/Repositories/System/MasterData/BranchRepository.cs
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
            => await _db.Branches
                .Include(x => x.Company)
                .Include(x => x.ParentBranch)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Branch?> GetByCodeAsync(string code)
            => await _db.Branches.FirstOrDefaultAsync(x => x.Code == code);

        public async Task<Branch?> GetByCodeAsync(string code, int companyId)
            => await _db.Branches.FirstOrDefaultAsync(x => x.Code == code && x.CompanyId == companyId);

        public async Task<List<Branch>> GetAllAsync(int companyId)
            => await _db.Branches
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .Include(x => x.ParentBranch)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Branch>> GetByCompanyIdAsync(int companyId)
            => await _db.Branches
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Branch>> GetByParentIdAsync(int parentId)
            => await _db.Branches
                .Where(x => x.CancelDate == null && x.ParentId == parentId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

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
            var item = await _db.Branches.FindAsync(id);
            if (item != null) _db.Branches.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Branches.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code, int companyId)
            => await _db.Branches.AnyAsync(x => x.Code == code && x.CompanyId == companyId);

        public async Task<bool> CodeExistsAsync(string code, int companyId, int excludeId)
            => await _db.Branches.AnyAsync(x => x.Code == code && x.CompanyId == companyId && x.Id != excludeId);

        public async Task<List<Branch>> GetActiveBranchesAsync()
            => await _db.Branches
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

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
                query = query.Where(x => x.CompanyId == companyId.Value);

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
            var item = await _db.Branches.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Branches.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Branches
                .Where(x => x.CompanyId == companyId && x.CancelDate == null)
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
    }
}