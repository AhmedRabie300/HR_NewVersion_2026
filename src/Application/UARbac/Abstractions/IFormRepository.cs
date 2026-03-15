// Application/UARbac/Abstractions/IFormRepository.cs
using Domain.UARbac;
using Application.Common.Models;

namespace Application.UARbac.Abstractions
{
    public interface IFormRepository
    {
        // Basic CRUD
        Task<Form?> GetByIdAsync(int id);
        Task<List<Form>> GetAllAsync();
        Task<Form> AddAsync(Form form);
        Task UpdateAsync(Form form);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task SaveChangesAsync(CancellationToken ct);

        // Form specific queries
        Task<Form?> GetByCodeAsync(string code);
        Task<List<Form>> GetByModuleIdAsync(int moduleId);
        Task<PagedResult<Form>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);

        // Lookup forms
        Task<List<Form>> GetSearchFormsAsync();
        Task<List<Form>> GetMainFormsAsync();
    }
}