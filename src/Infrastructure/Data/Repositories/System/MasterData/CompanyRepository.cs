 using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _db.Companies
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Company?> GetByCodeAsync(string code)
        {
            return await _db.Companies
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<List<Company>> GetAllAsync()
        {
            return await _db.Companies
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Company> AddAsync(Company company)
        {
            await _db.Companies.AddAsync(company);
            return company;
        }

        public Task UpdateAsync(Company company)
        {
            _db.Companies.Update(company);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var company = await _db.Companies.FindAsync(id);
            if (company != null)
            {
                _db.Companies.Remove(company);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Companies.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _db.Companies.AnyAsync(x => x.Code == code);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            return await _db.Companies
                .AnyAsync(x => x.Code == code && x.Id != excludeId);
        }

        public async Task<List<Company>> GetActiveCompaniesAsync()
        {
            return await _db.Companies
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedResult<Company>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Company> query = _db.Companies
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

            return new PagedResult<Company>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var company = await _db.Companies.FindAsync(id);
            if (company != null)
            {
                company.Cancel(regUserId);
                _db.Companies.Update(company);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }
    }
}