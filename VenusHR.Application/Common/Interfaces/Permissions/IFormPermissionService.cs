using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenusHR.Application.Common.DTOs.Permissions;

namespace VenusHR.Application.Common.Interfaces.Permissions
{
    public interface IFormPermissionService
    {
         Task<List<FormPermissionDto>> GetUserPermissionsAsync(int userId);
        Task<List<FormPermissionDto>> GetGroupPermissionsAsync(int groupId);
        Task<FormPermissionDto?> GetPermissionByIdAsync(int id);

         Task<FormPermissionDto> CreateUserPermissionAsync(CreateUserPermissionDto dto);
        Task<FormPermissionDto> CreateGroupPermissionAsync(CreateGroupPermissionDto dto);

       
        Task<List<FormPermissionDto>> UpdateGroupPermissionsAsync(int groupId, List<UpdateGroupPermissionDto> permissions);

         Task<List<FormPermissionDto>> UpdateUserPermissionsAsync(int userId, List<UpdateUserPermissionDto> permissions);

  
        Task<bool> DeleteAllGroupPermissionsAsync(int groupId);
 
        Task<bool> DeleteAllUserPermissionsAsync(int userId);

         Task<bool> CheckUserPermissionAsync(int userId, int formId, string action);
        Task<Dictionary<int, bool>> CheckUserMultiplePermissionsAsync(int userId, List<int> formIds, string action);
        Task<List<int>> GetUserAllowedFormsAsync(int userId, string action);

         Task<List<FormPermissionDto>> AssignUserPermissionsAsync(int userId, List<CreateUserPermissionDto> permissions);
        Task<List<FormPermissionDto>> AssignGroupPermissionsAsync(int groupId, List<CreateGroupPermissionDto> permissions);

         Task<bool> PermissionExistsAsync(int id);
        Task<bool> HasUserPermissionAsync(int userId, int formId);
        Task<bool> HasGroupPermissionAsync(int groupId, int formId);
    }
}