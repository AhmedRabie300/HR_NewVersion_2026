using Application.Abstractions;
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS.Employees;
using Infrastructure.Common.CurrentUser;
using Infrastructure.Common.Helpers;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Infrastructure.Common.Helpers;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Data.Repositories.System.HRS.Employees
{
    public sealed class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUser _currentUser;

        public EmployeeRepository(ApplicationDbContext db, ICurrentUser currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<Employee?> GetByIdAsync(int id)
            => await _db.Employees
                .Include(x => x.Company)
                .Include(x => x.BirthCity)
                .Include(x => x.Religion)
                .Include(x => x.MaritalStatus)
                .Include(x => x.BloodGroup)
                .Include(x => x.Bank)
                .Include(x => x.Nationality)
                .Include(x => x.Department)
                .Include(x => x.Branch)
                .Include(x => x.Sponsor)
                .Include(x => x.Sector)
                .Include(x => x.Location)
                .Include(x => x.Manager)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Employee?> GetByCodeAsync(string code)
            => await _db.Employees
                .Include(x => x.Company)
                .Include(x => x.BirthCity)
                .Include(x => x.Religion)
                .Include(x => x.MaritalStatus)
                .Include(x => x.BloodGroup)
                .Include(x => x.Bank)
                .Include(x => x.Nationality)
                .Include(x => x.Department)
                .Include(x => x.Branch)
                .Include(x => x.Sponsor)
                .Include(x => x.Sector)
                .Include(x => x.Location)
                .Include(x => x.Manager)
                .FirstOrDefaultAsync(x => x.Code == code);

        public async Task<Employee?> GetBySSnNoAsync(string ssnNo)
            => await _db.Employees
                .FirstOrDefaultAsync(x => x.SSnNo == ssnNo && x.CancelDate == null);

        public async Task<List<Employee>> GetAllAsync()
        {
            var companyId = _currentUser.CompanyId;
            return await _db.Employees
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .Include(x => x.Department)
                .Include(x => x.Branch)
                .Include(x => x.Nationality)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Employee>> GetByCompanyIdAsync()
        {
            var companyId = _currentUser.CompanyId;
            return await _db.Employees
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .Include(x => x.Department)
                .Include(x => x.Branch)
                .Include(x => x.Nationality)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Employee> AddAsync(Employee entity)
        {
            await _db.Employees.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Employee entity)
        {
            _db.Employees.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Employees.FindAsync(id);
            if (item != null) _db.Employees.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Employees.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.Employees.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.Employees.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode && x.Id != excludeId);
        }

        public async Task<bool> SSnNoExistsAsync(string ssnNo)
        {
            if (string.IsNullOrWhiteSpace(ssnNo)) return false;
            return await _db.Employees.AnyAsync(x => x.SSnNo == ssnNo && x.CancelDate == null);
        }

        public async Task<bool> SSnNoExistsAsync(string ssnNo, int excludeId)
        {
            if (string.IsNullOrWhiteSpace(ssnNo)) return false;
            return await _db.Employees.AnyAsync(x => x.SSnNo == ssnNo && x.CancelDate == null && x.Id != excludeId);
        }

        public async Task<PagedResult<Employee>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var companyId = _currentUser.CompanyId;
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Employee> query = _db.Employees
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .Include(x => x.Department)
                .Include(x => x.Branch)
                .Include(x => x.Nationality)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(x =>
                    (x.EngName != null && x.EngName.Contains(searchTerm)) ||
                    (x.ArbName != null && x.ArbName.Contains(searchTerm)) ||
                    x.Code.Contains(searchTerm) ||
                    (x.SSnNo != null && x.SSnNo.Contains(searchTerm)));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(x => x.Code)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Employee>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.Employees.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.Employees.Update(item);
            }
        }

        public async Task<string?> GetNextCodeAsync(int prefixType, int? referenceId, string separator, int padLength, CancellationToken ct)
        {
            var companyId = _currentUser.CompanyId;

            // Get all codes from active employees in the company
            var allCodes = await _db.Employees
                .Where(x => x.CancelDate == null && x.CompanyId == companyId && x.Code != null)
                .Select(x => x.Code)
                .ToListAsync(ct);

            // Extract numbers from codes (after removing prefix if exists)
            var numbers = new List<int>();
            string? prefix = await GetPrefixValue(prefixType, referenceId, ct);

            foreach (var code in allCodes)
            {
                var codeWithoutPrefix = code;

                if (!string.IsNullOrEmpty(prefix))
                {
                    if (code.StartsWith(prefix))
                    {
                        codeWithoutPrefix = code.Substring(prefix.Length);
                        if (!string.IsNullOrEmpty(separator) && codeWithoutPrefix.StartsWith(separator))
                            codeWithoutPrefix = codeWithoutPrefix.Substring(separator.Length);
                    }
                }

                var number = ExtractNumberFromCode(codeWithoutPrefix);
                if (number > 0) numbers.Add(number);
            }

            var nextNumber = numbers.Any() ? numbers.Max() + 1 : 1;
            string? newPrefix = await GetPrefixValue(prefixType, referenceId, ct);
            string prefixPart = string.IsNullOrEmpty(newPrefix) ? "" : newPrefix + separator;

            return $"{prefixPart}{nextNumber.ToString().PadLeft(padLength, '0')}";
        }

        private async Task<string?> GetPrefixValue(int prefixType, int? referenceId, CancellationToken ct)
        {
            switch (prefixType)
            {
                case 0: return "";
                case 1: // Branch
                    if (referenceId.HasValue)
                    {
                        var branch = await _db.Branches.FindAsync(new object[] { referenceId.Value }, ct);
                        return branch?.Code;
                    }
                    return "";
                case 2: // Department
                    if (referenceId.HasValue)
                    {
                        var department = await _db.Departments.FindAsync(new object[] { referenceId.Value }, ct);
                        return department?.Code;
                    }
                    return "";
                case 3: // Position
                    if (referenceId.HasValue)
                    {
                        var position = await _db.Positions.FindAsync(new object[] { referenceId.Value }, ct);
                        return position?.Code;
                    }
                    return "";
                case 4: // ContractType
                    if (referenceId.HasValue)
                    {
                        var contractType = await _db.ContractsTypes.FindAsync(new object[] { referenceId.Value }, ct);
                        return contractType?.Code;
                    }
                    return "";
                default: return "";
            }
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
            var query = _db.Employees
                .Where(x => x.CancelDate == null && x.EngName != null && x.EngName.Trim().ToLower() == trimmedEngName.ToLower());
            if (excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync(ct);
        }

        public async Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(arbName)) return true;
            var trimmedArbName = arbName.Trim();
            var query = _db.Employees
                .Where(x => x.CancelDate == null && x.ArbName != null && x.ArbName.Trim() == trimmedArbName);
            if (excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync(ct);
        }

        public async Task<Employee?> GetManagerByCodeAsync(string managerCode)
        {
            if (string.IsNullOrWhiteSpace(managerCode)) return null;
            return await _db.Employees
                .FirstOrDefaultAsync(x => x.Code == managerCode && x.CancelDate == null);
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);


public async Task<string?> GetListJsonAsync(int pageNumber, int pageSize, string? orderBy, string? orderDirection, string? criteria)
    {
        var userId = _currentUser.UserId ?? 1;
        var lang = _currentUser.Language == 2 ? "AR" : "EN";
        var companyId = _currentUser.CompanyId;

         JObject criteriaObj;
        if (string.IsNullOrEmpty(criteria))
        {
            criteriaObj = new JObject();
        }
        else
        {
            criteriaObj = JObject.Parse(criteria);
        }
        criteriaObj["companyid"] = companyId;
        criteria = criteriaObj.ToString();

        return await DataHelper.ExecuteListProcedureAsync(
            "hrs_EmployeesGetList",
            userId,
            0,
            "/basics/employees",
            lang,
            pageSize,
            pageNumber,
            orderBy,
            orderDirection,
            criteria);
    }
}
}