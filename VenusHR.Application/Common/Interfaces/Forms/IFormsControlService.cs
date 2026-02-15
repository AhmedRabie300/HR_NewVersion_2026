using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::VenusHR.Application.Common.DTOs.Forms;
namespace VenusHR.Application.Common.Interfaces.Forms

{
    public interface IFormsControlService
    {
         Task<List<FormsControlDto>> GetAllControlsAsync();
        Task<FormsControlDto?> GetControlByIdAsync(int id);
        Task<List<FormsControlDto>> GetControlsByFormIdAsync(int formId);
        Task<List<FormsControlDto>> GetVisibleControlsByFormIdAsync(int formId);

         Task<FormsControlDto> CreateControlAsync(CreateFormsControlDto dto);

         Task<FormsControlDto> UpdateControlAsync(int id, UpdateFormsControlDto dto);

         Task<bool> DeleteControlAsync(int id);
        Task<bool> SoftDeleteControlAsync(int id);

         Task<List<FormsControlDto>> CreateBulkControlsAsync(List<CreateFormsControlDto> dtos);
        Task<bool> DeleteControlsByFormIdAsync(int formId);

         Task<bool> ControlExistsAsync(int id);
        Task<int> GetMaxRankAsync(int formId);
        Task<bool> ReorderControlsAsync(int formId, Dictionary<int, int> controlRanks);
    }
}