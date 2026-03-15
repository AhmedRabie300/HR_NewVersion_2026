
using Application.Common.Models;
using Application.UARbac.Abstractions;
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.UARbac
{
    public sealed class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _db;

        public MenuRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // Basic CRUD
        public async Task<Menu?> GetByIdAsync(int id)
        {
            return await _db.Menus
                .Include(x => x.Parent)
                 .Include(x => x.Children)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Menu>> GetAllAsync()
        {
            return await _db.Menus
                         .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Menu> AddAsync(Menu menu)
        {
            await _db.Menus.AddAsync(menu);
            return menu;
        }

        public Task UpdateAsync(Menu menu)
        {
            _db.Menus.Update(menu);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var menu = await _db.Menus.FindAsync(id);
            if (menu != null)
            {
                // Check if has children
                var hasChildren = await _db.Menus.AnyAsync(x => x.ParentId == id);
                if (hasChildren)
                    throw new Exception("Cannot delete menu that has children");

                _db.Menus.Remove(menu);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Menus.AnyAsync(x => x.Id == id);
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }

        // Menu specific queries
        public async Task<Menu?> GetByCodeAsync(string code)
        {
            return await _db.Menus
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<List<Menu>> GetByParentIdAsync(int? parentId)
        {
            return await _db.Menus
                .Where(x => x.ParentId == parentId)
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Menu>> GetRootMenusAsync()
        {
            return await _db.Menus
                .Where(x => x.ParentId == null && x.CancelDate == null)
                .Include(x => x.Children)
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Menu>> GetMenuHierarchyAsync()
        {
             var allMenus = await _db.Menus
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

            return allMenus.Where(x => x.ParentId == null).ToList();
        }

        public async Task<PagedResult<Menu>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Menu> query = _db.Menus
                .Include(x => x.Parent)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(x =>
                    x.EngName != null && x.EngName.Contains(searchTerm) ||
                    x.ArbName != null && x.ArbName.Contains(searchTerm) ||
                    x.Code != null && x.Code.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Menu>(items, pageNumber, pageSize, totalCount);
        }

        // User menus (هتحتاج Join مع UserGroups و GroupPermissions)
        public async Task<List<Menu>> GetUserMenusAsync(int userId)
        {
            // مؤقتاً: جلب كل القوائم
            // هتتعدل بعد ما نعمل Permissions
            return await _db.Menus
                .Where(x => x.CancelDate == null)
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Menu>> GetUserVisibleMenusAsync(int userId)
        {
             return await _db.Menus
                .Where(x => x.CancelDate == null && x.IsHide != true)
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

         public async Task<Menu?> GetByFormIdAsync(int formId)
        {
            return await _db.Menus
                .FirstOrDefaultAsync(x => x.FormId == formId);
        }

        public async Task<List<Menu>> GetByFormIdsAsync(List<int> formIds)
        {
            return await _db.Menus
                .Where(x => formIds.Contains(x.FormId ?? 0))
                .AsNoTracking()
                .ToListAsync();
        }

        // Bulk operations
        public async Task UpdateRanksAsync(Dictionary<int, int> menuRanks)
        {
            foreach (var item in menuRanks)
            {
                var menu = await _db.Menus.FindAsync(item.Key);
                if (menu != null)
                {
                     
                }
            }
        }
    }
}