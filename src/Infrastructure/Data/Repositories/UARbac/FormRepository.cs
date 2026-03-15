using Application.Common.Models;
using Application.UARbac.Abstractions;
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.UARbac
{
    public sealed class FormRepository : IFormRepository
    {
        private readonly ApplicationDbContext _db;

        public FormRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // Basic CRUD
        public async Task<Form?> GetByIdAsync(int id)
        {
            return await _db.Forms
                //.Include(x => x.Module)
                .Include(x => x.SearchForm)
                .Include(x => x.MainForm)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Form>> GetAllAsync()
        {
            return await _db.Forms
            //    .Include(x => x.Module)
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Form> AddAsync(Form form)
        {
            await _db.Forms.AddAsync(form);
            return form;
        }

        public Task UpdateAsync(Form form)
        {
            _db.Forms.Update(form);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var form = await _db.Forms.FindAsync(id);
            if (form != null)
            {
                // Check if has permissions
                var hasPermissions = await _db.FormPermissions.AnyAsync(x => x.FormId == id);
                if (hasPermissions)
                    throw new Exception("Cannot delete form that has permissions");

                _db.Forms.Remove(form);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Forms.AnyAsync(x => x.Id == id);
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }

        // Form specific queries
        public async Task<Form?> GetByCodeAsync(string code)
        {
            return await _db.Forms
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<List<Form>> GetByModuleIdAsync(int moduleId)
        {
            return await _db.Forms
                .Where(x => x.ModuleId == moduleId && x.CancelDate == null)
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedResult<Form>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Form> query = _db.Forms
            //    .Include(x => x.Module)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(x =>
                    (x.EngName != null && x.EngName.Contains(searchTerm)) ||
                    (x.ArbName != null && x.ArbName.Contains(searchTerm)) ||
                    (x.Code != null && x.Code.Contains(searchTerm)));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Form>(items, pageNumber, pageSize, totalCount);
        }

        // Lookup forms
        public async Task<List<Form>> GetSearchFormsAsync()
        {
            return await _db.Forms
                .Where(x => x.SearchFormId == null && x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Form>> GetMainFormsAsync()
        {
            return await _db.Forms
                .Where(x => x.MainId == null && x.CancelDate == null)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}