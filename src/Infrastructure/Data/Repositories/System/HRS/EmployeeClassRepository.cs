using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS.Basics.GradesAndClasses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.HRS
{
    public sealed class EmployeeClassRepository : IEmployeeClassRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeClassRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // ==================== EmployeeClass (Master) ====================

        public async Task<EmployeeClass?> GetByIdAsync(int id)
            => await _db.EmployeeClasses
                .Include(x => x.Company)
                .Include(x => x.DefaultProject)
                .Include(x => x.Delays)
                .Include(x => x.Vacations)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<EmployeeClass?> GetByCodeAsync(string code)
            => await _db.EmployeeClasses
                .Include(x => x.Company)
                .Include(x => x.DefaultProject)
                .Include(x => x.Delays)
                .Include(x => x.Vacations)
                .FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<EmployeeClass>> GetAllAsync()
            => await _db.EmployeeClasses
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.DefaultProject)
                .Include(x => x.Delays)
                .Include(x => x.Vacations)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<EmployeeClass>> GetByCompanyIdAsync()
            => await _db.EmployeeClasses
                .Where(x => x.CancelDate == null )
                .Include(x => x.Company)
                .Include(x => x.DefaultProject)
                .Include(x => x.Delays)
                .Include(x => x.Vacations)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();

        public async Task<EmployeeClass> AddAsync(EmployeeClass entity)
        {
            await _db.EmployeeClasses.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(EmployeeClass entity)
        {
            _db.EmployeeClasses.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.EmployeeClasses.FindAsync(id);
            if (item != null) _db.EmployeeClasses.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.EmployeeClasses.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.EmployeeClasses.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.EmployeeClasses.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode && x.Id != excludeId);
        }

        public async Task<PagedResult<EmployeeClass>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<EmployeeClass> query = _db.EmployeeClasses
                .Where(x => x.CancelDate == null)
                .Include(x => x.Company)
                .Include(x => x.DefaultProject)
                .AsNoTracking();

 

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(x =>
                    (x.EngName != null && x.EngName.Contains(searchTerm)) ||
                    (x.ArbName != null && x.ArbName.Contains(searchTerm)) ||
                    x.Code.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(x => x.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<EmployeeClass>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.EmployeeClasses.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.EmployeeClasses.Update(item);
            }
        }

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.EmployeeClasses
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Select(x => x.Code)
                .ToListAsync(ct);

            if (!allCodes.Any())
                return null;

            var maxCode = allCodes
                .Select(code => new { Code = code, Number = ExtractNumberFromCode(code) })
                .Where(x => x.Number > 0)
                .OrderByDescending(x => x.Number)
                .FirstOrDefault();

            return maxCode?.Code;
        }

        private int ExtractNumberFromCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return 0;
            var match = Regex.Match(code, @"\d+$");
            if (match.Success && int.TryParse(match.Value, out int number))
                return number;
            if (int.TryParse(code, out int directNumber))
                return directNumber;
            return 0;
        }

        public async Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(engName)) return true;
            var trimmedEngName = engName.Trim();
            var query = _db.EmployeeClasses
                .Where(x => x.CancelDate == null && x.EngName != null && x.EngName.Trim().ToLower() == trimmedEngName.ToLower());
            if (excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync(ct);
        }

        public async Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(arbName)) return true;
            var trimmedArbName = arbName.Trim();
            var query = _db.EmployeeClasses
                .Where(x => x.CancelDate == null && x.ArbName != null && x.ArbName.Trim() == trimmedArbName);
            if (excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync(ct);
        }

        // ==================== EmployeeClassDelay (Detail 1) ====================

        public async Task<EmployeeClassDelay?> GetDelayByIdAsync(int id)
            => await _db.EmployeeClassDelays
                .Include(x => x.EmployeeClass)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<EmployeeClassDelay>> GetDelaysByClassIdAsync(int classId)
            => await _db.EmployeeClassDelays
                .Where(x => x.ClassId == classId && x.CancelDate == null)
                .OrderBy(x => x.FromMin)
                .AsNoTracking()
                .ToListAsync();

        public async Task<EmployeeClassDelay> AddDelayAsync(EmployeeClassDelay entity)
        {
            await _db.EmployeeClassDelays.AddAsync(entity);
            return entity;
        }

        public Task UpdateDelayAsync(EmployeeClassDelay entity)
        {
            _db.EmployeeClassDelays.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteDelayAsync(int id)
        {
            var item = await _db.EmployeeClassDelays.FindAsync(id);
            if (item != null) _db.EmployeeClassDelays.Remove(item);
        }

        public async Task<bool> DelayExistsAsync(int id)
            => await _db.EmployeeClassDelays.AnyAsync(x => x.Id == id);

        public async Task SoftDeleteDelayAsync(int id, int? regUserId = null)
        {
            var item = await _db.EmployeeClassDelays.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.EmployeeClassDelays.Update(item);
            }
        }

        // ==================== EmployeeClassVacation (Detail 2) ====================

        public async Task<EmployeeClassVacation?> GetVacationByIdAsync(int id)
            => await _db.EmployeeClassVacations
                .Include(x => x.EmployeeClass)
                .Include(x => x.VacationType)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<EmployeeClassVacation>> GetVacationsByClassIdAsync(int classId)
            => await _db.EmployeeClassVacations
                .Where(x => x.EmployeeClassId == classId && x.CancelDate == null)
                .OrderBy(x => x.VacationTypeId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<EmployeeClassVacation> AddVacationAsync(EmployeeClassVacation entity)
        {
            await _db.EmployeeClassVacations.AddAsync(entity);
            return entity;
        }

        public Task UpdateVacationAsync(EmployeeClassVacation entity)
        {
            _db.EmployeeClassVacations.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteVacationAsync(int id)
        {
            var item = await _db.EmployeeClassVacations.FindAsync(id);
            if (item != null) _db.EmployeeClassVacations.Remove(item);
        }

        public async Task<bool> VacationExistsAsync(int id)
            => await _db.EmployeeClassVacations.AnyAsync(x => x.Id == id);

        public async Task SoftDeleteVacationAsync(int id, int? regUserId = null)
        {
            var item = await _db.EmployeeClassVacations.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.EmployeeClassVacations.Update(item);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}