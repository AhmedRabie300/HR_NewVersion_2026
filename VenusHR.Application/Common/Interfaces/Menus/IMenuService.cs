using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenusHR.Application.Common.DTOs.Menus;

namespace VenusHR.Application.Common.Interfaces.Menus
{
    public interface IMenuService
    {
         Task<List<MenuDto>> GetAllMenusAsync();
        Task<MenuDto?> GetMenuByIdAsync(int id);
        Task<List<MenuDto>> GetMenuTreeAsync();
        Task<List<MenuDto>> GetUserMenusAsync(int userId);

         Task<MenuDto> CreateMenuAsync(CreateMenuDto dto);

         Task<MenuDto> UpdateMenuAsync(int id, UpdateMenuDto dto);

         Task<bool> DeleteMenuAsync(int id);

         Task<bool> MenuExistsAsync(int id);
        Task<bool> IsMenuCodeUniqueAsync(string code);
    }
}