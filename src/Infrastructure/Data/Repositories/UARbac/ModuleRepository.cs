// Infrastructure/Repositories/UARbac/ModuleRepository.cs
using Application.Common.Models;
using Application.UARbac.Abstractions;
using Domain.UARbac;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.UARbac
{
    public sealed class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext _db;

        public ModuleRepository(ApplicationDbContext db)
        {
            _db = db;
        }

 
        public async Task<Module?> GetByIdAsync(int id)
        {
            return await _db.Modules
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Module>> GetAllAsync()
        {
            return await _db.Modules
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Module> AddAsync(Module module)
        {
            await _db.Modules.AddAsync(module);
            return module;
        }

        public Task UpdateAsync(Module module)
        {
            _db.Modules.Update(module);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var module = await _db.Modules.FindAsync(id);
            if (module != null)
            {
                _db.Modules.Remove(module);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Modules.AnyAsync(x => x.Id == id);
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }

        // ========== Module Specific Queries ==========

        public async Task<Module?> GetByCodeAsync(string code)
        {
            return await _db.Modules
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _db.Modules.AnyAsync(x => x.Code == code);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            return await _db.Modules
                .AnyAsync(x => x.Code == code && x.Id != excludeId);
        }

        public async Task<List<Module>> GetActiveModulesAsync()
        {
            return await _db.Modules
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedResult<Module>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Module> query = _db.Modules.AsNoTracking();

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
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Module>(items, pageNumber, pageSize, totalCount);
        }

        public async Task<List<Module>> GetModulesByTypeAsync(string moduleType)
        {
            IQueryable<Module> query = _db.Modules.Where(x => x.CancelDate == null);

            switch (moduleType.ToUpper())
            {
                case "HR":
                    query = query.Where(x => x.IsHR == true);
                    break;
                case "GL":
                    query = query.Where(x => x.IsGL == true);
                    break;
                case "AR":
                    query = query.Where(x => x.IsAR == true);
                    break;
                case "AP":
                    query = query.Where(x => x.IsAP == true);
                    break;
                case "FA":
                    query = query.Where(x => x.IsFA == true);
                    break;
                case "INV":
                    query = query.Where(x => x.IsINV == true);
                    break;
                case "MANF":
                    query = query.Where(x => x.IsMANF == true);
                    break;
                case "SYS":
                    query = query.Where(x => x.IsSYS == true);
                    break;
                default:
                    return new List<Module>();
            }

            return await query
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Module?> GetByFormIdAsync(int formId)
        {
            return await _db.Modules
                .FirstOrDefaultAsync(x => x.FormId == formId);
        }


        public async Task SoftDeleteAsync(int id)
        {
            var module = await _db.Modules.FindAsync(id);
            if (module != null)
            {
         
                module.Cancel(null);
                _db.Modules.Update(module);
            }
        }

        public async Task RestoreAsync(int id)
        {
            var module = await _db.Modules.FindAsync(id);
            if (module != null)
            {
                
                typeof(Module).GetProperty("CancelDate")?.SetValue(module, null);
                _db.Modules.Update(module);
            }
        }
    }
}