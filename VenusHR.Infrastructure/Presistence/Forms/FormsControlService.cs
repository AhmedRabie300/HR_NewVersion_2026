using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::VenusHR.Application.Common.DTOs.Forms;
using global::VenusHR.Application.Common.Interfaces.Forms;
using global::VenusHR.Core.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace VenusHR.Infrastructure.Presistence.Forms
 
{
    public class FormsControlService : IFormsControlService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<FormsControlService> _logger;

        public FormsControlService(ApplicationDBContext context, ILogger<FormsControlService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ============ GET ============

        public async Task<List<FormsControlDto>> GetAllControlsAsync()
        {
            try
            {
                var controls = await _context.sys_FormsControls
                    .Where(c => c.CancelDate == null)
                    .Include(c => c.Form)
                    .OrderBy(c => c.FormID)
                    .ThenBy(c => c.Rank)
                    .Select(c => new FormsControlDto
                    {
                        Id = c.ID,
                        FormId = c.FormID,
                        Name = c.Name,
                        EngCaption = c.EngCaption,
                        ArbCaption = c.ArbCaption,
                        Compulsory = c.Compulsory == 1,
                        Format = c.Format,
                        ArbFormat = c.ArbFormat,
                        EngToolTip = c.EngToolTip,
                        ArbToolTip = c.ArbToolTip,
                        MaxLength = c.MaxLength,
                        IsNumeric = c.IsNumeric == 1,
                        IsHide = c.IsHide == 1,
                        FocusOnStartUp = c.FocusOnStartUp == 1,
                        Rank = c.Rank,
                        MinValue = c.MinValue,
                        MaxValue = c.MaxValue,
                        FieldId = c.FieldID,
                        SearchId = c.SearchID,
                        IsArabic = c.IsArabic == 1,
                        FormName = c.Form != null ? c.Form.EngName : null
                    })
                    .ToListAsync();

                return controls;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all form controls");
                return new List<FormsControlDto>();
            }
        }

        public async Task<FormsControlDto?> GetControlByIdAsync(int id)
        {
            try
            {
                var control = await _context.sys_FormsControls
                    .Where(c => c.ID == id && c.CancelDate == null)
                    .Include(c => c.Form)
                    .Select(c => new FormsControlDto
                    {
                        Id = c.ID,
                        FormId = c.FormID,
                        Name = c.Name,
                        EngCaption = c.EngCaption,
                        ArbCaption = c.ArbCaption,
                        Compulsory = c.Compulsory == 1,
                        Format = c.Format,
                        ArbFormat = c.ArbFormat,
                        EngToolTip = c.EngToolTip,
                        ArbToolTip = c.ArbToolTip,
                        MaxLength = c.MaxLength,
                        IsNumeric = c.IsNumeric == 1,
                        IsHide = c.IsHide == 1,
                        FocusOnStartUp = c.FocusOnStartUp == 1,
                        Rank = c.Rank,
                        MinValue = c.MinValue,
                        MaxValue = c.MaxValue,
                        FieldId = c.FieldID,
                        SearchId = c.SearchID,
                        IsArabic = c.IsArabic == 1,
                        FormName = c.Form != null ? c.Form.EngName : null
                    })
                    .FirstOrDefaultAsync();

                return control;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting control by id {Id}", id);
                return null;
            }
        }

        public async Task<List<FormsControlDto>> GetControlsByFormIdAsync(int formId)
        {
            try
            {
                var controls = await _context.sys_FormsControls
                    .Where(c => c.FormID == formId && c.CancelDate == null)
                    .OrderBy(c => c.Rank)
                    .Select(c => new FormsControlDto
                    {
                        Id = c.ID,
                        FormId = c.FormID,
                        Name = c.Name,
                        EngCaption = c.EngCaption,
                        ArbCaption = c.ArbCaption,
                        Compulsory = c.Compulsory == 1,
                        Format = c.Format,
                        ArbFormat = c.ArbFormat,
                        EngToolTip = c.EngToolTip,
                        ArbToolTip = c.ArbToolTip,
                        MaxLength = c.MaxLength,
                        IsNumeric = c.IsNumeric == 1,
                        IsHide = c.IsHide == 1,
                        FocusOnStartUp = c.FocusOnStartUp == 1,
                        Rank = c.Rank,
                        MinValue = c.MinValue,
                        MaxValue = c.MaxValue,
                        FieldId = c.FieldID,
                        SearchId = c.SearchID,
                        IsArabic = c.IsArabic == 1
                    })
                    .ToListAsync();

                return controls;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting controls for form {FormId}", formId);
                return new List<FormsControlDto>();
            }
        }

        public async Task<List<FormsControlDto>> GetVisibleControlsByFormIdAsync(int formId)
        {
            try
            {
                var controls = await _context.sys_FormsControls
                    .Where(c => c.FormID == formId && c.CancelDate == null && c.IsHide != 1)
                    .OrderBy(c => c.Rank)
                    .Select(c => new FormsControlDto
                    {
                        Id = c.ID,
                        FormId = c.FormID,
                        Name = c.Name,
                        EngCaption = c.EngCaption,
                        ArbCaption = c.ArbCaption,
                        Compulsory = c.Compulsory == 1,
                        Format = c.Format,
                        MaxLength = c.MaxLength,
                        IsNumeric = c.IsNumeric == 1,
                        Rank = c.Rank
                    })
                    .ToListAsync();

                return controls;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting visible controls for form {FormId}", formId);
                return new List<FormsControlDto>();
            }
        }

        // ============ CREATE ============

        public async Task<FormsControlDto> CreateControlAsync(CreateFormsControlDto dto)
        {
            try
            {
                var control = new sys_FormsControls
                {
                    FormID = dto.FormId,
                    Name = dto.Name,
                    EngCaption = dto.EngCaption,
                    ArbCaption = dto.ArbCaption,
                    Compulsory = dto.Compulsory == true ? 1 : 0,
                    Format = dto.Format,
                    ArbFormat = dto.ArbFormat,
                    EngToolTip = dto.EngToolTip,
                    ArbToolTip = dto.ArbToolTip,
                    MaxLength = dto.MaxLength,
                    IsNumeric = dto.IsNumeric == true ? 1 : 0,
                    IsHide = dto.IsHide == true ? 1 : 0,
                    FocusOnStartUp = dto.FocusOnStartUp == true ? 1 : 0,
                    Rank = dto.Rank ?? await GetNextRankAsync(dto.FormId),
                    MinValue = dto.MinValue,
                    MaxValue = dto.MaxValue,
                    FieldID = dto.FieldId,
                    SearchID = dto.SearchId,
                    IsArabic = dto.IsArabic == true ? 1 : 0,
                    RegDate = DateTime.Now
                };

                await _context.sys_FormsControls.AddAsync(control);
                await _context.SaveChangesAsync();

                return await GetControlByIdAsync(control.ID) ?? new FormsControlDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating form control");
                throw;
            }
        }

        // ============ UPDATE ============

        public async Task<FormsControlDto> UpdateControlAsync(int id, UpdateFormsControlDto dto)
        {
            try
            {
                var control = await _context.sys_FormsControls.FindAsync(id);
                if (control == null)
                    throw new Exception($"Control with id {id} not found");

                if (dto.FormId != null) control.FormID = dto.FormId.Value;
                if (dto.Name != null) control.Name = dto.Name;
                if (dto.EngCaption != null) control.EngCaption = dto.EngCaption;
                if (dto.ArbCaption != null) control.ArbCaption = dto.ArbCaption;
                if (dto.Compulsory != null) control.Compulsory = dto.Compulsory.Value ? 1 : 0;
                if (dto.Format != null) control.Format = dto.Format;
                if (dto.ArbFormat != null) control.ArbFormat = dto.ArbFormat;
                if (dto.EngToolTip != null) control.EngToolTip = dto.EngToolTip;
                if (dto.ArbToolTip != null) control.ArbToolTip = dto.ArbToolTip;
                if (dto.MaxLength != null) control.MaxLength = dto.MaxLength;
                if (dto.IsNumeric != null) control.IsNumeric = dto.IsNumeric.Value ? 1 : 0;
                if (dto.IsHide != null) control.IsHide = dto.IsHide.Value ? 1 : 0;
                if (dto.FocusOnStartUp != null) control.FocusOnStartUp = dto.FocusOnStartUp.Value ? 1 : 0;
                if (dto.Rank != null) control.Rank = dto.Rank;
                if (dto.MinValue != null) control.MinValue = dto.MinValue;
                if (dto.MaxValue != null) control.MaxValue = dto.MaxValue;
                if (dto.FieldId != null) control.FieldID = dto.FieldId;
                if (dto.SearchId != null) control.SearchID = dto.SearchId;
                if (dto.IsArabic != null) control.IsArabic = dto.IsArabic.Value ? 1 : 0;

                await _context.SaveChangesAsync();

                return await GetControlByIdAsync(id) ?? new FormsControlDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating control {Id}", id);
                throw;
            }
        }

        // ============ DELETE ============

        public async Task<bool> DeleteControlAsync(int id)
        {
            try
            {
                var control = await _context.sys_FormsControls.FindAsync(id);
                if (control == null)
                    return false;

                _context.sys_FormsControls.Remove(control);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting control {Id}", id);
                return false;
            }
        }

        public async Task<bool> SoftDeleteControlAsync(int id)
        {
            try
            {
                var control = await _context.sys_FormsControls.FindAsync(id);
                if (control == null)
                    return false;

                control.CancelDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting control {Id}", id);
                return false;
            }
        }

        // ============ BULK ============

        public async Task<List<FormsControlDto>> CreateBulkControlsAsync(List<CreateFormsControlDto> dtos)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var controls = dtos.Select(d => new sys_FormsControls
                {
                    FormID = d.FormId,
                    Name = d.Name,
                    EngCaption = d.EngCaption,
                    ArbCaption = d.ArbCaption,
                    Compulsory = d.Compulsory == true ? 1 : 0,
                    Format = d.Format,
                    ArbFormat = d.ArbFormat,
                    EngToolTip = d.EngToolTip,
                    ArbToolTip = d.ArbToolTip,
                    MaxLength = d.MaxLength,
                    IsNumeric = d.IsNumeric == true ? 1 : 0,
                    IsHide = d.IsHide == true ? 1 : 0,
                    FocusOnStartUp = d.FocusOnStartUp == true ? 1 : 0,
                    Rank = d.Rank,
                    MinValue = d.MinValue,
                    MaxValue = d.MaxValue,
                    FieldID = d.FieldId,
                    SearchID = d.SearchId,
                    IsArabic = d.IsArabic == true ? 1 : 0,
                    RegDate = DateTime.Now
                }).ToList();

                await _context.sys_FormsControls.AddRangeAsync(controls);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var result = new List<FormsControlDto>();
                foreach (var control in controls)
                {
                    var dto = await GetControlByIdAsync(control.ID);
                    if (dto != null) result.Add(dto);
                }

                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating bulk controls");
                throw;
            }
        }

        public async Task<bool> DeleteControlsByFormIdAsync(int formId)
        {
            try
            {
                var controls = await _context.sys_FormsControls
                    .Where(c => c.FormID == formId)
                    .ToListAsync();

                _context.sys_FormsControls.RemoveRange(controls);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting controls for form {FormId}", formId);
                return false;
            }
        }

        // ============ HELPERS ============

        public async Task<bool> ControlExistsAsync(int id)
        {
            return await _context.sys_FormsControls
                .AnyAsync(c => c.ID == id && c.CancelDate == null);
        }

        public async Task<int> GetMaxRankAsync(int formId)
        {
            var maxRank = await _context.sys_FormsControls
                .Where(c => c.FormID == formId && c.CancelDate == null)
                .MaxAsync(c => (int?)c.Rank) ?? 0;

            return maxRank + 1;
        }

        private async Task<int?> GetNextRankAsync(int formId)
        {
            return await GetMaxRankAsync(formId);
        }

        public async Task<bool> ReorderControlsAsync(int formId, Dictionary<int, int> controlRanks)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in controlRanks)
                {
                    var control = await _context.sys_FormsControls.FindAsync(item.Key);
                    if (control != null && control.FormID == formId)
                    {
                        control.Rank = item.Value;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error reordering controls for form {FormId}", formId);
                return false;
            }
        }
    }
}