// Infrastructure/Data/Repositories/System/MasterData/ContractTypeRepository.cs
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class ContractTypeRepository : IContractTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public ContractTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ContractType?> GetByIdAsync(int id)
            => await _db.ContractTypes
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<ContractType?> GetByCodeAsync(string code, int companyId)
            => await _db.ContractTypes
                .FirstOrDefaultAsync(x => x.Code == code && x.CompanyId == companyId);

        public async Task<List<ContractType>> GetAllAsync(int companyId)
            => await _db.ContractTypes
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<ContractType>> GetByCompanyIdAsync(int companyId)
            => await _db.ContractTypes
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<ContractType> AddAsync(ContractType entity)
        {
            await _db.ContractTypes.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(ContractType entity)
        {
            _db.ContractTypes.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.ContractTypes.FindAsync(id);
            if (item != null) _db.ContractTypes.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.ContractTypes.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code, int companyId)
            => await _db.ContractTypes.AnyAsync(x => x.Code == code && x.CompanyId == companyId);

        public async Task<bool> CodeExistsAsync(string code, int companyId, int excludeId)
            => await _db.ContractTypes.AnyAsync(x => x.Code == code && x.CompanyId == companyId && x.Id != excludeId);

        public async Task<List<ContractType>> GetActiveContractTypesAsync()
            => await _db.ContractTypes
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<PagedResult<ContractType>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, int? companyId)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<ContractType> query = _db.ContractTypes
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
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

            return new PagedResult<ContractType>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.ContractTypes.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.ContractTypes.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}