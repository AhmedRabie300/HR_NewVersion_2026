using Application.UARbac.Abstractions;
using Domain.UARbac;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories.UARbac;

public sealed class FormControlRepository : IFormControlRepository
{
    private readonly ApplicationDbContext _db;
    public FormControlRepository(ApplicationDbContext db) => _db = db;

    public Task<FormControl?> GetByIdAsync(int id, CancellationToken ct)
        => _db.FormControls.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<List<FormControl>> ListByFormIdAsync(int formId, int? section, CancellationToken ct)
    {
        var query = _db.FormControls
            .AsNoTracking()
            .Where(x => x.FormId == formId && x.CancelDate == null);

        if (section.HasValue)
            query = query.Where(x => x.Section == section.Value);

        return query
            .OrderBy(x => x.Id)
            .ToListAsync(ct);
    }
    public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
