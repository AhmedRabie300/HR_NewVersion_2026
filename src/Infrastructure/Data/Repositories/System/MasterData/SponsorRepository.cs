using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class SponsorRepository : ISponsorRepository
    {
        private readonly ApplicationDbContext _db;

        public SponsorRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
            => await _db.Sponsors
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Sponsor?> GetByCodeAsync(string code, int companyId)
            => await _db.Sponsors.FirstOrDefaultAsync(x => x.Code == code && x.CompanyId == companyId);

        public async Task<List<Sponsor>> GetAllAsync(int companyId)
            => await _db.Sponsors
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<Sponsor>> GetByCompanyIdAsync(int companyId)
            => await _db.Sponsors
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Sponsor> AddAsync(Sponsor entity)
        {
            await _db.Sponsors.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Sponsor entity)
        {
            _db.Sponsors.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Sponsors.FindAsync(id);
            if (item != null) _db.Sponsors.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Sponsors.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code, int companyId)
            => await _db.Sponsors.AnyAsync(x => x.Code == code && x.CompanyId == companyId);

        public async Task<bool> CodeExistsAsync(string code, int companyId, int excludeId)
            => await _db.Sponsors.AnyAsync(x => x.Code == code && x.CompanyId == companyId && x.Id != excludeId);

        public async Task<PagedResult<Sponsor>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Sponsor> query = _db.Sponsors
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

            return new PagedResult<Sponsor>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.Sponsors.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Sponsors.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}