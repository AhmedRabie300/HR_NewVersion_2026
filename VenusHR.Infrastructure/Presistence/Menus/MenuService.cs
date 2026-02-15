 using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VenusHR.Application.Common.DTOs;
using VenusHR.Application.Common.DTOs.Menus;
using VenusHR.Application.Common.Interfaces.Menus;
using VenusHR.Core;
using VenusHR.Core.Login;
using VenusHR.Infrastructure.Presistence;
using WorkFlow_EF;

namespace VenusHR.Infrastructure.Services
{
    public class MenuService : IMenuService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<MenuService> _logger;

        public MenuService(ApplicationDBContext context, ILogger<MenuService> logger)
        {
            _context = context;
            _logger = logger;
        }

         public async Task<List<MenuDto>> GetAllMenusAsync()
        {
            try
            {
                var menus = await _context.sys_Menus
                    .Where(m => m.CancelDate == null)
                    .OrderBy(m => m.Rank)
                    .Select(m => new MenuDto
                    {
                        Id = m.ID,
                        Code = m.Code ?? "",
                        EngName = m.EngName ?? "",
                        ArbName = m.ArbName ?? "",
                        ArbName4S = m.ArbName4S,
                        ParentId = m.ParentID,
                        Shortcut = m.Shortcut,
                        Rank = m.Rank,
                        FormId = m.FormID,
                        ObjectId = m.ObjectID,
                        ViewFormId = m.ViewFormID,
                        IsHide = m.IsHide == 1,
                        Image = m.Image,
                        ViewType = m.ViewType
                    })
                    .ToListAsync();

                return menus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all menus");
                return new List<MenuDto>();
            }
        }

        public async Task<MenuDto?> GetMenuByIdAsync(int id)
        {
            try
            {
                var menu = await _context.sys_Menus
                    .Where(m => m.ID == id && m.CancelDate == null)
                    .Select(m => new MenuDto
                    {
                        Id = m.ID,
                        Code = m.Code ?? "",
                        EngName = m.EngName ?? "",
                        ArbName = m.ArbName ?? "",
                        ArbName4S = m.ArbName4S,
                        ParentId = m.ParentID,
                        Shortcut = m.Shortcut,
                        Rank = m.Rank,
                        FormId = m.FormID,
                        ObjectId = m.ObjectID,
                        ViewFormId = m.ViewFormID,
                        IsHide = m.IsHide == 1,
                        Image = m.Image,
                        ViewType = m.ViewType
                    })
                    .FirstOrDefaultAsync();

                return menu;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting menu by id {Id}", id);
                return null;
            }
        }

        public async Task<List<MenuDto>> GetMenuTreeAsync()
        {
            try
            {
                var allMenus = await GetAllMenusAsync();
                return BuildMenuTree(allMenus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting menu tree");
                return new List<MenuDto>();
            }
        }

        public async Task<List<MenuDto>> GetUserMenusAsync(int userId)
        {
            try
            {
                 var userGroups = await _context.Sys_GroupsUsers
                    .Where(gu => gu.UserId == userId)
                    .Select(gu => gu.GroupId)
                    .ToListAsync();

                 var userForms = await _context.sys_FormsPermissions
                    .Where(fp => (fp.UserID == userId || (fp.GroupID != null && userGroups.Contains(fp.GroupID.Value)))
                                && fp.CancelDate == null
                                && (fp.AllowView == true || fp.AllowAdd == true || fp.AllowEdit == true))
                    .Select(fp => fp.FormID)
                    .Distinct()
                    .ToListAsync();

                 var menus = await _context.sys_Menus
                    .Where(m => m.CancelDate == null &&
                               (m.FormID == null || userForms.Contains(m.FormID.Value)))
                    .OrderBy(m => m.Rank)
                    .Select(m => new MenuDto
                    {
                        Id = m.ID,
                        Code = m.Code ?? "",
                        EngName = m.EngName ?? "",
                        ArbName = m.ArbName ?? "",
                        ParentId = m.ParentID,
                        Rank = m.Rank,
                        FormId = m.FormID,
                        IsHide = m.IsHide == 1,
                        Image = m.Image,

                         Form = m.Form != null ? new FormDto
                        {
                            Id = m.Form.ID,
                            Code = m.Form.Code ?? "",
                            EngName = m.Form.EngName ?? "",
                            ArbName = m.Form.ArbName ?? "",
                            ModuleId = m.Form.ModuleID,

                             Module = m.Form.Module != null ? new ModuleDto
                            {
                                Id = m.Form.Module.ID,
                                EngName = m.Form.Module.EngName ?? "",
                                ArbName = m.Form.Module.ArbName ?? "",
                                Rank = m.Form.Module.Rank ?? 0,
                                Code = m.Form.Module.Code ?? ""
                            } : null
                        } : null
                    })
                    .ToListAsync();

                 return BuildMenuTree(menus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting menus for user {UserId}", userId);
                return new List<MenuDto>();
            }
        }
        private List<MenuDto> BuildMenuTree(List<MenuDto> menus)
        {
            var menuDict = menus.ToDictionary(m => m.Id);
            var rootMenus = new List<MenuDto>();

            foreach (var menu in menus)
            {
                if (menu.ParentId == null || !menuDict.ContainsKey(menu.ParentId.Value))
                {
                    rootMenus.Add(menu);
                }
                else
                {
                    var parent = menuDict[menu.ParentId.Value];
                    parent.Children.Add(menu);
                }
            }

             foreach (var menu in menuDict.Values)
            {
                menu.Children = menu.Children.OrderBy(c => c.Rank).ToList();
            }

            return rootMenus.OrderBy(m => m.Rank).ToList();
        }

 
        public async Task<MenuDto> CreateMenuAsync(CreateMenuDto dto)
        {
            try
            {
                 if (!await IsMenuCodeUniqueAsync(dto.Code))
                    throw new Exception($"Menu code '{dto.Code}' already exists");

                var menu = new sys_Menus
                {
                    Code = dto.Code,
                    EngName = dto.EngName,
                    ArbName = dto.ArbName,
                    ArbName4S = dto.ArbName4S,
                    ParentID = dto.ParentId,
                    Shortcut = dto.Shortcut,
                    Rank = dto.Rank ?? await GetNextRankAsync(dto.ParentId),
                    FormID = dto.FormId,
                    ObjectID = dto.ObjectId,
                    ViewFormID = dto.ViewFormId,
                    IsHide = dto.IsHide ? 1 : 0,
                    Image = dto.Image,
                    ViewType = dto.ViewType,
                    RegDate = DateTime.Now
                };

                await _context.sys_Menus.AddAsync(menu);
                await _context.SaveChangesAsync();

                return await GetMenuByIdAsync(menu.ID) ?? new MenuDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu");
                throw;
            }
        }

 
        public async Task<MenuDto> UpdateMenuAsync(int id, UpdateMenuDto dto)
        {
            try
            {
                var menu = await _context.sys_Menus.FindAsync(id);
                if (menu == null)
                    throw new Exception($"Menu with id {id} not found");

                 if (dto.Code != null && dto.Code != menu.Code)
                {
                    if (!await IsMenuCodeUniqueAsync(dto.Code))
                        throw new Exception($"Menu code '{dto.Code}' already exists");
                }

                if (dto.Code != null) menu.Code = dto.Code;
                if (dto.EngName != null) menu.EngName = dto.EngName;
                if (dto.ArbName != null) menu.ArbName = dto.ArbName;
                if (dto.ArbName4S != null) menu.ArbName4S = dto.ArbName4S;
                if (dto.ParentId != null) menu.ParentID = dto.ParentId;
                if (dto.Shortcut != null) menu.Shortcut = dto.Shortcut;
                if (dto.Rank != null) menu.Rank = dto.Rank;
                if (dto.FormId != null) menu.FormID = dto.FormId;
                if (dto.ObjectId != null) menu.ObjectID = dto.ObjectId;
                if (dto.ViewFormId != null) menu.ViewFormID = dto.ViewFormId;
                if (dto.IsHide != null) menu.IsHide = dto.IsHide.Value ? 1 : 0;
                if (dto.Image != null) menu.Image = dto.Image;
                if (dto.ViewType != null) menu.ViewType = dto.ViewType;

                await _context.SaveChangesAsync();

                return await GetMenuByIdAsync(id) ?? new MenuDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu {Id}", id);
                throw;
            }
        }

 
        public async Task<bool> DeleteMenuAsync(int id)
        {
            try
            {
                var menu = await _context.sys_Menus.FindAsync(id);
                if (menu == null)
                    return false;

                 menu.CancelDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu {Id}", id);
                return false;
            }
        }

 
        public async Task<bool> MenuExistsAsync(int id)
        {
            return await _context.sys_Menus
                .AnyAsync(m => m.ID == id && m.CancelDate == null);
        }

        public async Task<bool> IsMenuCodeUniqueAsync(string code)
        {
            return !await _context.sys_Menus
                .AnyAsync(m => m.Code == code && m.CancelDate == null);
        }

        private async Task<int?> GetNextRankAsync(int? parentId)
        {
            var maxRank = await _context.sys_Menus
                .Where(m => m.ParentID == parentId && m.CancelDate == null)
                .MaxAsync(m => (int?)m.Rank) ?? 0;

            return maxRank + 1;
        }
    }
}