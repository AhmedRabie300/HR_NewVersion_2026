using Domain.UARbac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UARbac.Abstractions
{
    public interface IFormControlRepository
    {
        Task<FormControl?> GetByIdAsync(int id, CancellationToken ct);

        Task<List<FormControl>> ListByFormIdAsync(int formId, int? section,CancellationToken ct);

        Task SaveChangesAsync(CancellationToken ct);
    }
}
