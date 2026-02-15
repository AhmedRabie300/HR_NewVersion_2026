using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenusHR.Application.Common.DTOs;
using VenusHR.Application.Common.DTOs.Forms;
using VenusHR.Application.Common.DTOs.Permissions;
using VenusHR.Application.Common.Interfaces.Permissions;
using VenusHR.Core.Login;
using VenusHR.Infrastructure.Presistence;
using WorkFlow_EF;

namespace VenusHR.Infrastructure.Services
{
    public class FormPermissionService : IFormPermissionService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<FormPermissionService> _logger;

        public FormPermissionService(ApplicationDBContext context, ILogger<FormPermissionService> logger)
        {
            _context = context;
            _logger = logger;
        }

 
        public async Task<List<FormPermissionDto>> GetUserPermissionsAsync(int userId)
        {
            try
            {
                var permissions = await _context.sys_FormsPermissions
                    .Where(fp => fp.UserID == userId && fp.CancelDate == null)
                    .Include(fp => fp.Form)
                    .Select(fp => new FormPermissionDto
                    {
                        Id = fp.ID,
                        FormId = fp.FormID,
                        UserId = fp.UserID,
                        AllowView = fp.AllowView ?? false,
                        AllowAdd = fp.AllowAdd ?? false,
                        AllowEdit = fp.AllowEdit ?? false,
                        AllowDelete = fp.AllowDelete ?? false,
                        AllowPrint = fp.AllowPrint ?? false,
                        Form = fp.Form != null ? new FormDto
                        {
                            Id = fp.Form.ID,
                            Code = fp.Form.Code,
                            EngName = fp.Form.EngName ?? "",
                            ArbName = fp.Form.ArbName ?? "",
                            ModuleId = fp.Form.ModuleID,
                            LinkUrl = fp.Form.LinkUrl,
                            ImageUrl = fp.Form.ImageUrl
                        } : null
                    })
                    .ToListAsync();

                return permissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permissions for user {UserId}", userId);
                return new List<FormPermissionDto>();
            }
        }

        public async Task<List<FormPermissionDto>> GetGroupPermissionsAsync(int groupId)
        {
            try
            {
                var permissions = await _context.sys_FormsPermissions
                    .Where(fp => fp.GroupID == groupId && fp.CancelDate == null)
                    .Include(fp => fp.Form)
                    .Select(fp => new FormPermissionDto
                    {
                        Id = fp.ID,
                        FormId = fp.FormID,
                        GroupId = fp.GroupID,
                        AllowView = fp.AllowView ?? false,
                        AllowAdd = fp.AllowAdd ?? false,
                        AllowEdit = fp.AllowEdit ?? false,
                        AllowDelete = fp.AllowDelete ?? false,
                        AllowPrint = fp.AllowPrint ?? false,
                        Form = fp.Form != null ? new FormDto
                        {
                            Id = fp.Form.ID,
                            Code = fp.Form.Code,
                            EngName = fp.Form.EngName ?? "",
                            ArbName = fp.Form.ArbName ?? "",
                            ModuleId = fp.Form.ModuleID,
                            LinkUrl = fp.Form.LinkUrl,
                            ImageUrl = fp.Form.ImageUrl
                        } : null
                    })
                    .ToListAsync();

                return permissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permissions for group {GroupId}", groupId);
                return new List<FormPermissionDto>();
            }
        }

        public async Task<FormPermissionDto?> GetPermissionByIdAsync(int id)
        {
            try
            {
                var permission = await _context.sys_FormsPermissions
                    .Where(fp => fp.ID == id && fp.CancelDate == null)
                    .Include(fp => fp.Form)
                    .Select(fp => new FormPermissionDto
                    {
                        Id = fp.ID,
                        FormId = fp.FormID,
                        GroupId = fp.GroupID,
                        UserId = fp.UserID,
                        AllowView = fp.AllowView ?? false,
                        AllowAdd = fp.AllowAdd ?? false,
                        AllowEdit = fp.AllowEdit ?? false,
                        AllowDelete = fp.AllowDelete ?? false,
                        AllowPrint = fp.AllowPrint ?? false,
                        Form = fp.Form != null ? new FormDto
                        {
                            Id = fp.Form.ID,
                            Code = fp.Form.Code,
                            EngName = fp.Form.EngName ?? "",
                            ArbName = fp.Form.ArbName ?? "",
                            ModuleId = fp.Form.ModuleID,
                            LinkUrl = fp.Form.LinkUrl,
                            ImageUrl = fp.Form.ImageUrl
                        } : null
                    })
                    .FirstOrDefaultAsync();

                return permission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting permission by id {Id}", id);
                return null;
            }
        }

 
        public async Task<FormPermissionDto> CreateUserPermissionAsync(CreateUserPermissionDto dto)
        {
            try
            {
                var permission = new sys_FormsPermissions
                {
                    UserID = dto.UserId,
                    FormID = dto.FormId,
                    AllowView = dto.AllowView,
                    AllowAdd = dto.AllowAdd,
                    AllowEdit = dto.AllowEdit,
                    AllowDelete = dto.AllowDelete,
                    AllowPrint = dto.AllowPrint,
                    RegDate = DateTime.Now
                };

                await _context.sys_FormsPermissions.AddAsync(permission);
                await _context.SaveChangesAsync();

                return await GetPermissionByIdAsync(permission.ID) ?? new FormPermissionDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user permission");
                throw;
            }
        }

        public async Task<FormPermissionDto> CreateGroupPermissionAsync(CreateGroupPermissionDto dto)
        {
            try
            {
                var permission = new sys_FormsPermissions
                {
                    GroupID = dto.GroupId,
                    FormID = dto.FormId,
                    AllowView = dto.AllowView,
                    AllowAdd = dto.AllowAdd,
                    AllowEdit = dto.AllowEdit,
                    AllowDelete = dto.AllowDelete,
                    AllowPrint = dto.AllowPrint,
                    RegDate = DateTime.Now
                };

                await _context.sys_FormsPermissions.AddAsync(permission);
                await _context.SaveChangesAsync();

                return await GetPermissionByIdAsync(permission.ID) ?? new FormPermissionDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating group permission");
                throw;
            }
        }

 
        public async Task<List<FormPermissionDto>> UpdateGroupPermissionsAsync(int groupId, List<UpdateGroupPermissionDto> permissions)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                 var oldPermissions = await _context.sys_FormsPermissions
                    .Where(fp => fp.GroupID == groupId)
                    .ToListAsync();

                _context.sys_FormsPermissions.RemoveRange(oldPermissions);

                 var newPermissions = permissions.Select(p => new sys_FormsPermissions
                {
                    GroupID = groupId,
                    FormID = p.FormId,
                    AllowView = p.AllowView,
                    AllowAdd = p.AllowAdd,
                    AllowEdit = p.AllowEdit,
                    AllowDelete = p.AllowDelete,
                    AllowPrint = p.AllowPrint,
                    RegDate = DateTime.Now
                }).ToList();

                await _context.sys_FormsPermissions.AddRangeAsync(newPermissions);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                 return newPermissions.Select(fp => new FormPermissionDto
                {
                    Id = fp.ID,
                    FormId = fp.FormID,
                    GroupId = fp.GroupID,
                    AllowView = fp.AllowView ?? false,
                    AllowAdd = fp.AllowAdd ?? false,
                    AllowEdit = fp.AllowEdit ?? false,
                    AllowDelete = fp.AllowDelete ?? false,
                    AllowPrint = fp.AllowPrint ?? false
                }).ToList();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error updating permissions for group {GroupId}", groupId);
                throw;
            }
        }

        public async Task<List<FormPermissionDto>> UpdateUserPermissionsAsync(int userId, List<UpdateUserPermissionDto> permissions)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                 var oldPermissions = await _context.sys_FormsPermissions
                    .Where(fp => fp.UserID == userId)
                    .ToListAsync();

                _context.sys_FormsPermissions.RemoveRange(oldPermissions);

                 var newPermissions = permissions.Select(p => new sys_FormsPermissions
                {
                    UserID = userId,
                    FormID = p.FormId,
                    AllowView = p.AllowView,
                    AllowAdd = p.AllowAdd,
                    AllowEdit = p.AllowEdit,
                    AllowDelete = p.AllowDelete,
                    AllowPrint = p.AllowPrint,
                    RegDate = DateTime.Now
                }).ToList();

                await _context.sys_FormsPermissions.AddRangeAsync(newPermissions);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return newPermissions.Select(fp => new FormPermissionDto
                {
                    Id = fp.ID,
                    FormId = fp.FormID,
                    UserId = fp.UserID,
                    AllowView = fp.AllowView ?? false,
                    AllowAdd = fp.AllowAdd ?? false,
                    AllowEdit = fp.AllowEdit ?? false,
                    AllowDelete = fp.AllowDelete ?? false,
                    AllowPrint = fp.AllowPrint ?? false
                }).ToList();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error updating permissions for user {UserId}", userId);
                throw;
            }
        }
         public async Task<bool> DeletePermissionAsync(int id)
        {
            try
            {
                var permission = await _context.sys_FormsPermissions.FindAsync(id);
                if (permission == null)
                    return false;

                permission.CancelDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting permission {Id}", id);
                return false;
            }
        }

        public async Task<bool> DeleteAllGroupPermissionsAsync(int groupId)
        {
            try
            {
                var permissions = await _context.sys_FormsPermissions
                    .Where(fp => fp.GroupID == groupId)
                    .ToListAsync();

                foreach (var permission in permissions)
                {
                    permission.CancelDate = DateTime.Now;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting all permissions for group {GroupId}", groupId);
                return false;
            }
        }

        public async Task<bool> DeleteAllUserPermissionsAsync(int userId)
        {
            try
            {
                var permissions = await _context.sys_FormsPermissions
                    .Where(fp => fp.UserID == userId)
                    .ToListAsync();

                foreach (var permission in permissions)
                {
                    permission.CancelDate = DateTime.Now;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting all permissions for user {UserId}", userId);
                return false;
            }
        }

 
        public async Task<bool> CheckUserPermissionAsync(int userId, int formId, string action)
        {
            try
            {
                 var userPermission = await _context.sys_FormsPermissions
                    .Where(fp => fp.UserID == userId && fp.FormID == formId && fp.CancelDate == null)
                    .FirstOrDefaultAsync();

                if (userPermission != null)
                {
                    var result = CheckAction(userPermission, action);
                    if (result) return true;
                }

                 var userGroups = await _context.Sys_GroupsUsers
                    .Where(gu => gu.UserId == userId)
                    .Select(gu => gu.GroupId)
                    .ToListAsync();

                if (userGroups.Any())
                {
                    var groupPermissions = await _context.sys_FormsPermissions
                        .Where(fp => userGroups.Contains(fp.GroupID ?? 0) && fp.FormID == formId && fp.CancelDate == null)
                        .ToListAsync();

                    foreach (var gp in groupPermissions)
                    {
                        if (CheckAction(gp, action))
                            return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking permission for user {UserId}, form {FormId}", userId, formId);
                return false;
            }
        }

        private bool CheckAction(sys_FormsPermissions permission, string action)
        {
            return action.ToLower() switch
            {
                "view" => permission.AllowView ?? false,
                "add" => permission.AllowAdd ?? false,
                "edit" => permission.AllowEdit ?? false,
                "delete" => permission.AllowDelete ?? false,
                "print" => permission.AllowPrint ?? false,
                _ => false
            };
        }

        public async Task<Dictionary<int, bool>> CheckUserMultiplePermissionsAsync(int userId, List<int> formIds, string action)
        {
            var result = new Dictionary<int, bool>();

            foreach (var formId in formIds)
            {
                result[formId] = await CheckUserPermissionAsync(userId, formId, action);
            }

            return result;
        }

        public async Task<List<int>> GetUserAllowedFormsAsync(int userId, string action)
        {
            try
            {
                var allForms = await _context.sys_Forms
                    .Where(f => f.CancelDate == null)
                    .Select(f => f.ID)
                    .ToListAsync();

                var allowedForms = new List<int>();

                foreach (var formId in allForms)
                {
                    if (await CheckUserPermissionAsync(userId, formId, action))
                        allowedForms.Add(formId);
                }

                return allowedForms;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting allowed forms for user {UserId}", userId);
                return new List<int>();
            }
        }

 
        public async Task<List<FormPermissionDto>> AssignUserPermissionsAsync(int userId, List<CreateUserPermissionDto> permissions)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newPermissions = permissions.Select(p => new sys_FormsPermissions
                {
                    UserID = userId,
                    FormID = p.FormId,
                    AllowView = p.AllowView,
                    AllowAdd = p.AllowAdd,
                    AllowEdit = p.AllowEdit,
                    AllowDelete = p.AllowDelete,
                    AllowPrint = p.AllowPrint,
                    RegDate = DateTime.Now
                }).ToList();

                await _context.sys_FormsPermissions.AddRangeAsync(newPermissions);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return newPermissions.Select(fp => new FormPermissionDto
                {
                    Id = fp.ID,
                    FormId = fp.FormID,
                    UserId = fp.UserID,
                    AllowView = fp.AllowView ?? false,
                    AllowAdd = fp.AllowAdd ?? false,
                    AllowEdit = fp.AllowEdit ?? false,
                    AllowDelete = fp.AllowDelete ?? false,
                    AllowPrint = fp.AllowPrint ?? false
                }).ToList();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error assigning permissions for user {UserId}", userId);
                throw;
            }
        }

        public async Task<List<FormPermissionDto>> AssignGroupPermissionsAsync(int groupId, List<CreateGroupPermissionDto> permissions)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newPermissions = permissions.Select(p => new sys_FormsPermissions
                {
                    GroupID = groupId,
                    FormID = p.FormId,
                    AllowView = p.AllowView,
                    AllowAdd = p.AllowAdd,
                    AllowEdit = p.AllowEdit,
                    AllowDelete = p.AllowDelete,
                    AllowPrint = p.AllowPrint,
                    RegDate = DateTime.Now
                }).ToList();

                await _context.sys_FormsPermissions.AddRangeAsync(newPermissions);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return newPermissions.Select(fp => new FormPermissionDto
                {
                    Id = fp.ID,
                    FormId = fp.FormID,
                    GroupId = fp.GroupID,
                    AllowView = fp.AllowView ?? false,
                    AllowAdd = fp.AllowAdd ?? false,
                    AllowEdit = fp.AllowEdit ?? false,
                    AllowDelete = fp.AllowDelete ?? false,
                    AllowPrint = fp.AllowPrint ?? false
                }).ToList();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error assigning permissions for group {GroupId}", groupId);
                throw;
            }
        }

 
        public async Task<bool> PermissionExistsAsync(int id)
        {
            return await _context.sys_FormsPermissions
                .AnyAsync(fp => fp.ID == id && fp.CancelDate == null);
        }

        public async Task<bool> HasUserPermissionAsync(int userId, int formId)
        {
            return await _context.sys_FormsPermissions
                .AnyAsync(fp => fp.UserID == userId && fp.FormID == formId && fp.CancelDate == null);
        }

        public async Task<bool> HasGroupPermissionAsync(int groupId, int formId)
        {
            return await _context.sys_FormsPermissions
                .AnyAsync(fp => fp.GroupID == groupId && fp.FormID == formId && fp.CancelDate == null);
        }
    }
}