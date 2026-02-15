using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenusHR.Application.Common.DTOs;
using VenusHR.Application.Common.DTOs.Forms;

namespace VenusHR.Application.Common.Interfaces.Forms
{
    public interface IFormService
    {
         Task<List<FormDto>> GetAllFormsAsync();
        Task<FormDto?> GetFormByIdAsync(int id);
        Task<List<FormDto>> GetFormsByModuleAsync(int moduleId);
        Task<List<FormDto>> GetUserFormsAsync(int userId);

         Task<FormDto> CreateFormAsync(CreateFormDto dto);

         Task<FormDto> UpdateFormAsync(int id, UpdateFormDto dto);

         Task<bool> DeleteFormAsync(int id);

         Task<bool> FormExistsAsync(int id);
        Task<bool> IsFormCodeUniqueAsync(string code);
    }
}