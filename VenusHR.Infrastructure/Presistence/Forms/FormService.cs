 using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VenusHR.Application.Common.DTOs;
using VenusHR.Application.Common.DTOs.Forms;
using VenusHR.Application.Common.Interfaces.Forms;
using VenusHR.Core;
using VenusHR.Core.Login;
using VenusHR.Infrastructure.Presistence;
using WorkFlow_EF;

namespace VenusHR.Infrastructure.Services
{
    public class FormService : IFormService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<FormService> _logger;

        public FormService(ApplicationDBContext context, ILogger<FormService> logger)
        {
            _context = context;
            _logger = logger;
        }

 
        public async Task<List<FormDto>> GetAllFormsAsync()
        {
            try
            {
                var forms = await _context.sys_Forms
                    .Where(f => f.CancelDate == null)
                    .OrderBy(f => f.ModuleID)
                    .ThenBy(f => f.Rank)
                    .Select(f => new FormDto
                    {
                        Id = f.ID,
                        Code = f.Code ?? "",
                        EngName = f.EngName ?? "",
                        ArbName = f.ArbName ?? "",
                        ArbName4S = f.ArbName4S,
                        EngDescription = f.EngDescription,
                        ArbDescription = f.ArbDescription,
                        Rank = f.Rank,
                        ModuleId = f.ModuleID,
                        SearchFormId = f.SearchFormID,
                        Height = f.Height,
                        Width = f.Width,
                        Remarks = f.Remarks,
                        Layout = f.Layout,
                        LinkTarget = f.LinkTarget,
                        LinkUrl = f.LinkUrl,
                        ImageUrl = f.ImageUrl,
                        MainId = f.MainID
                    })
                    .ToListAsync();

                return forms;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all forms");
                return new List<FormDto>();
            }
        }

        public async Task<FormDto?> GetFormByIdAsync(int id)
        {
            try
            {
                var form = await _context.sys_Forms
                    .Where(f => f.ID == id && f.CancelDate == null)
                    .Select(f => new FormDto
                    {
                        Id = f.ID,
                        Code = f.Code ?? "",
                        EngName = f.EngName ?? "",
                        ArbName = f.ArbName ?? "",
                        ArbName4S = f.ArbName4S,
                        EngDescription = f.EngDescription,
                        ArbDescription = f.ArbDescription,
                        Rank = f.Rank,
                        ModuleId = f.ModuleID,
                        SearchFormId = f.SearchFormID,
                        Height = f.Height,
                        Width = f.Width,
                        Remarks = f.Remarks,
                        Layout = f.Layout,
                        LinkTarget = f.LinkTarget,
                        LinkUrl = f.LinkUrl,
                        ImageUrl = f.ImageUrl,
                        MainId = f.MainID
                    })
                    .FirstOrDefaultAsync();

                return form;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting form by id {Id}", id);
                return null;
            }
        }

        public async Task<List<FormDto>> GetFormsByModuleAsync(int moduleId)
        {
            try
            {
                var forms = await _context.sys_Forms
                    .Where(f => f.ModuleID == moduleId && f.CancelDate == null)
                    .OrderBy(f => f.Rank)
                    .Select(f => new FormDto
                    {
                        Id = f.ID,
                        Code = f.Code ?? "",
                        EngName = f.EngName ?? "",
                        ArbName = f.ArbName ?? "",
                        ModuleId = f.ModuleID,
                        LinkUrl = f.LinkUrl,
                        ImageUrl = f.ImageUrl
                    })
                    .ToListAsync();

                return forms;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting forms for module {ModuleId}", moduleId);
                return new List<FormDto>();
            }
        }

        public async Task<List<FormDto>> GetUserFormsAsync(int userId)
        {
            try
            {
                 var userGroups = await _context.Sys_GroupsUsers
                    .Where(gu => gu.UserId == userId)
                    .Select(gu => gu.GroupId)
                    .ToListAsync();

                 var forms = await _context.sys_FormsPermissions
                    .Where(fp => (fp.UserID == userId || (fp.GroupID != null && userGroups.Contains(fp.GroupID.Value)))
                                && fp.CancelDate == null)
                    .Include(fp => fp.Form)
                    .Where(fp => fp.Form != null && fp.Form.CancelDate == null)
                    .Select(fp => new FormDto
                    {
                        Id = fp.Form.ID,
                        Code = fp.Form.Code ?? "",
                        EngName = fp.Form.EngName ?? "",
                        ArbName = fp.Form.ArbName ?? "",
                        ModuleId = fp.Form.ModuleID,
                        LinkUrl = fp.Form.LinkUrl,
                        ImageUrl = fp.Form.ImageUrl
                    })
                    .Distinct()
                    .ToListAsync();

                return forms;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting forms for user {UserId}", userId);
                return new List<FormDto>();
            }
        }

 
        public async Task<FormDto> CreateFormAsync(CreateFormDto dto)
        {
            try
            {
                var form = new sys_Forms
                {
                    Code = dto.Code,
                    EngName = dto.EngName,
                    ArbName = dto.ArbName,
                    ArbName4S = dto.ArbName4S,
                    EngDescription = dto.EngDescription,
                    ArbDescription = dto.ArbDescription,
                    Rank = dto.Rank,
                    ModuleID = dto.ModuleId,
                    SearchFormID = dto.SearchFormId,
                    Height = dto.Height,
                    Width = dto.Width,
                    Remarks = dto.Remarks,
                    Layout = dto.Layout,
                    LinkTarget = dto.LinkTarget,
                    LinkUrl = dto.LinkUrl,
                    ImageUrl = dto.ImageUrl,
                    MainID = dto.MainId,
                    RegDate = DateTime.Now
                };

                await _context.sys_Forms.AddAsync(form);
                await _context.SaveChangesAsync();

                return await GetFormByIdAsync(form.ID) ?? new FormDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating form");
                throw;
            }
        }

 
        public async Task<FormDto> UpdateFormAsync(int id, UpdateFormDto dto)
        {
            try
            {
                var form = await _context.sys_Forms.FindAsync(id);
                if (form == null)
                    throw new Exception($"Form with id {id} not found");

                if (dto.Code != null) form.Code = dto.Code;
                if (dto.EngName != null) form.EngName = dto.EngName;
                if (dto.ArbName != null) form.ArbName = dto.ArbName;
                if (dto.ArbName4S != null) form.ArbName4S = dto.ArbName4S;
                if (dto.EngDescription != null) form.EngDescription = dto.EngDescription;
                if (dto.ArbDescription != null) form.ArbDescription = dto.ArbDescription;
                if (dto.Rank != null) form.Rank = dto.Rank;
                if (dto.ModuleId != null) form.ModuleID = dto.ModuleId.Value;
                if (dto.SearchFormId != null) form.SearchFormID = dto.SearchFormId;
                if (dto.Height != null) form.Height = dto.Height;
                if (dto.Width != null) form.Width = dto.Width;
                if (dto.Remarks != null) form.Remarks = dto.Remarks;
                if (dto.Layout != null) form.Layout = dto.Layout;
                if (dto.LinkTarget != null) form.LinkTarget = dto.LinkTarget;
                if (dto.LinkUrl != null) form.LinkUrl = dto.LinkUrl;
                if (dto.ImageUrl != null) form.ImageUrl = dto.ImageUrl;
                if (dto.MainId != null) form.MainID = dto.MainId;

                await _context.SaveChangesAsync();

                return await GetFormByIdAsync(id) ?? new FormDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating form {Id}", id);
                throw;
            }
        }

 
        public async Task<bool> DeleteFormAsync(int id)
        {
            try
            {
                var form = await _context.sys_Forms.FindAsync(id);
                if (form == null)
                    return false;

                 form.CancelDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting form {Id}", id);
                return false;
            }
        }

 
        public async Task<bool> FormExistsAsync(int id)
        {
            return await _context.sys_Forms
                .AnyAsync(f => f.ID == id && f.CancelDate == null);
        }

        public async Task<bool> IsFormCodeUniqueAsync(string code)
        {
            return !await _context.sys_Forms
                .AnyAsync(f => f.Code == code && f.CancelDate == null);
        }
    }
}