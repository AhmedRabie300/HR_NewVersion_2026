using Domain.UARbac;
using Application.Common.Models;

namespace Application.UARbac.Abstractions
{
    public interface IModuleRepository
    {
 
    
        Task<Module?> GetByIdAsync(int id);

 
        Task<List<Module>> GetAllAsync();

 
        Task<Module> AddAsync(Module module);

      
        Task UpdateAsync(Module module);
 
        Task DeleteAsync(int id);

 
        Task<bool> ExistsAsync(int id);

 
        Task SaveChangesAsync(CancellationToken ct);

 
        Task<Module?> GetByCodeAsync(string code);

     
        Task<bool> CodeExistsAsync(string code);
 
        Task<bool> CodeExistsAsync(string code, int excludeId);

   
        Task<List<Module>> GetActiveModulesAsync();

    
        Task<PagedResult<Module>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);

 
        Task<List<Module>> GetModulesByTypeAsync(string moduleType);  

 
        Task<Module?> GetByFormIdAsync(int formId);

 
        Task SoftDeleteAsync(int id);

 
        Task RestoreAsync(int id);
    }
}