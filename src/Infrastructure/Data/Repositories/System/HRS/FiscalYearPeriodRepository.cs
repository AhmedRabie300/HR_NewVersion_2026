using Application.Abstractions;
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS.Basics;
using Domain.System.HRS.Basics.FiscalPeriod;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.HRS.Basics.FiscalPeriod
{
    public sealed class FiscalYearPeriodRepository : IFiscalYearPeriodRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUser _currentUser;

        public FiscalYearPeriodRepository(ApplicationDbContext db, ICurrentUser currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        // ==================== FiscalYearPeriod (Detail 1) ====================

        public async Task<FiscalYearPeriod?> GetByIdAsync(int id)
            => await _db.FiscalYearPeriods
                .Include(x => x.FiscalYear)
                .Include(x => x.Company)
                .Include(x => x.Modules)
                .ThenInclude(m => m.Module)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<FiscalYearPeriod?> GetByCodeAsync(string code)
            => await _db.FiscalYearPeriods
                .Include(x => x.FiscalYear)
                .Include(x => x.Company)
                .Include(x => x.Modules)
                .ThenInclude(m => m.Module)
                .FirstOrDefaultAsync(x => x.Code == code);

        public async Task<List<FiscalYearPeriod>> GetAllAsync()
            => await _db.FiscalYearPeriods
                .Where(x => x.CancelDate == null)
                .Include(x => x.FiscalYear)
                .Include(x => x.Company)
                .Include(x => x.Modules)
                .ThenInclude(m => m.Module)
                .OrderBy(x => x.PeriodRank)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<FiscalYearPeriod>> GetByFiscalYearIdAsync(int fiscalYearId)
            => await _db.FiscalYearPeriods
                .Where(x => x.CancelDate == null && x.FiscalYearId == fiscalYearId)
                .Include(x => x.FiscalYear)
                .Include(x => x.Company)
                .Include(x => x.Modules)
                .ThenInclude(m => m.Module)
                .OrderBy(x => x.PeriodRank)
                .AsNoTracking()
                .ToListAsync();

        public async Task<List<FiscalYearPeriod>> GetByCompanyIdAsync()
        {
            var companyId = _currentUser.CompanyId;
            return await _db.FiscalYearPeriods
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.FiscalYear)
                .Include(x => x.Company)
                .Include(x => x.Modules)
                .ThenInclude(m => m.Module)
                .OrderBy(x => x.PeriodRank)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<FiscalYearPeriod> AddAsync(FiscalYearPeriod entity)
        {
            await _db.FiscalYearPeriods.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(FiscalYearPeriod entity)
        {
            _db.FiscalYearPeriods.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.FiscalYearPeriods.FindAsync(id);
            if (item != null) _db.FiscalYearPeriods.Remove(item);
        }

        public async Task DeleteByFiscalYearIdAsync(int fiscalYearId, CancellationToken ct)
        {
            var items = await _db.FiscalYearPeriods
                .Where(x => x.FiscalYearId == fiscalYearId)
                .ToListAsync(ct);
            _db.FiscalYearPeriods.RemoveRange(items);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.FiscalYearPeriods.AnyAsync(x => x.Id == id);

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.FiscalYearPeriods.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            var trimmedCode = code.Trim();
            return await _db.FiscalYearPeriods.AnyAsync(x => x.Code != null && x.Code.Trim() == trimmedCode && x.Id != excludeId);
        }

        public async Task<PagedResult<FiscalYearPeriod>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var companyId = _currentUser.CompanyId;
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<FiscalYearPeriod> query = _db.FiscalYearPeriods
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.FiscalYear)
                .Include(x => x.Company)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(x =>
                    (x.EngName != null && x.EngName.Contains(searchTerm)) ||
                    (x.ArbName != null && x.ArbName.Contains(searchTerm)) ||
                    (x.Code != null && x.Code.Contains(searchTerm)));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(x => x.PeriodRank)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<FiscalYearPeriod>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var item = await _db.FiscalYearPeriods.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.FiscalYearPeriods.Update(item);
            }
        }

        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.FiscalYearPeriods
                .Where(x => x.CancelDate == null && x.CompanyId == companyId && x.Code != null)
                .Select(x => x.Code!)
                .ToListAsync(ct);

            if (!allCodes.Any()) return null;

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
            var query = _db.FiscalYearPeriods
                .Where(x => x.CancelDate == null && x.EngName != null && x.EngName.Trim().ToLower() == trimmedEngName.ToLower());
            if (excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync(ct);
        }

        public async Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(arbName)) return true;
            var trimmedArbName = arbName.Trim();
            var query = _db.FiscalYearPeriods
                .Where(x => x.CancelDate == null && x.ArbName != null && x.ArbName.Trim() == trimmedArbName);
            if (excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync(ct);
        }

        public async Task<List<FiscalYearPeriod>> GeneratePeriodsAsync(int fiscalYearId, bool isFormal, int? sourceFiscalYearId = null, CancellationToken ct = default)
        {
            var fiscalYear = await _db.FiscalYears.FirstOrDefaultAsync(x => x.Id == fiscalYearId, ct);
            if (fiscalYear == null)
                throw new Exception($"Fiscal year {fiscalYearId} not found");

            var companyId = _currentUser.CompanyId;
            var periods = new List<FiscalYearPeriod>();

            if (isFormal)
            {
                var fiscalYearNumber = int.Parse(fiscalYear.EngName ?? fiscalYear.ArbName ?? DateTime.Now.Year.ToString());
                var isHijri = await IsCompanyUsingHijriCalendar(companyId, ct);

                for (int month = 1; month <= 12; month++)
                {
                    DateTime fromDate, toDate, prepareFromDate, prepareToDate;
                    string hFromDate, hToDate;

                    if (isHijri)
                    {
                        fromDate = new DateTime(fiscalYearNumber, month, 1);
                        toDate = new DateTime(fiscalYearNumber, month, GetHijriMonthDays(month));
                        hFromDate = fromDate.ToString("dd/MM/yyyy");
                        hToDate = toDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        var daysInMonth = DateTime.DaysInMonth(fiscalYearNumber, month);
                        fromDate = new DateTime(fiscalYearNumber, month, 1);
                        toDate = new DateTime(fiscalYearNumber, month, daysInMonth);
                        hFromDate = fromDate.ToString("dd/MM/yyyy");
                        hToDate = toDate.ToString("dd/MM/yyyy");
                    }

                    // Prepare dates
                    var prepareYear = month == 1 ? fiscalYearNumber - 1 : fiscalYearNumber;
                    var prepareMonth = month == 1 ? 12 : month - 1;
                    prepareFromDate = new DateTime(prepareYear, prepareMonth, 25);
                    prepareToDate = new DateTime(fiscalYearNumber, month, 24);

                    var period = new FiscalYearPeriod(
                        code: $"P{month:D2}",
                        fiscalYearId: fiscalYearId,
                        engName: GetEnglishMonthName(month, isHijri) + " " + fiscalYearNumber,
                        arbName: GetArabicMonthName(month, isHijri) + " " + fiscalYearNumber,
                        arbName4S: GetArabicMonthName(month, isHijri),
                        fromDate: fromDate,
                        toDate: toDate,
                        remarks: null,
                        hFromDate: hFromDate,
                        hToDate: hToDate,
                        periodType: (byte)month,
                        periodRank: (byte)month,
                        prepareFromDate: prepareFromDate,
                        prepareToDate: prepareToDate,
                        companyId: companyId,
                        regComputerId: null
                    );
                    periods.Add(period);
                }
            }
            else if (sourceFiscalYearId.HasValue)
            {
                var sourcePeriods = await _db.FiscalYearPeriods
                    .Where(x => x.FiscalYearId == sourceFiscalYearId.Value && x.CancelDate == null)
                    .ToListAsync(ct);

                foreach (var source in sourcePeriods)
                {
                    var period = new FiscalYearPeriod(
                        code: source.Code,
                        fiscalYearId: fiscalYearId,
                        engName: source.EngName,
                        arbName: source.ArbName,
                        arbName4S: source.ArbName4S,
                        fromDate: source.FromDate,
                        toDate: source.ToDate,
                        remarks: source.Remarks,
                        hFromDate: source.HFromDate,
                        hToDate: source.HToDate,
                        periodType: source.PeriodType,
                        periodRank: source.PeriodRank,
                        prepareFromDate: source.PrepareFromDate,
                        prepareToDate: source.PrepareToDate,
                        companyId: companyId,
                        regComputerId: null
                    );
                    periods.Add(period);
                }
            }

            return periods;
        }

        private async Task<bool> IsCompanyUsingHijriCalendar(int companyId, CancellationToken ct)
        {
            var company = await _db.Companies
                .Where(x => x.Id == companyId)
                .Select(x => x.IsHigry)
                .FirstOrDefaultAsync(ct);
            return company == true;
        }

        private int GetHijriMonthDays(int month) => month switch
        {
            1 => 30,
            2 => 29,
            3 => 30,
            4 => 29,
            5 => 30,
            6 => 29,
            7 => 30,
            8 => 29,
            9 => 30,
            10 => 29,
            11 => 30,
            12 => 29,
            _ => 30
        };

        private string GetEnglishMonthName(int month, bool isHijri)
        {
            if (isHijri)
                return new[] { "", "Muharram", "Safar", "Rabi' al-awwal", "Rabi' al-thani",
                    "Jumada al-awwal", "Jumada al-thani", "Rajab", "Sha'ban", "Ramadan",
                    "Shawwal", "Dhu al-Qi'dah", "Dhu al-Hijjah" }[month];
            return new[] { "", "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December" }[month];
        }

        private string GetArabicMonthName(int month, bool isHijri)
        {
            if (isHijri)
                return new[] { "", "محرم", "صفر", "ربيع الأول", "ربيع الآخر", "جمادى الأولى",
                    "جمادى الآخرة", "رجب", "شعبان", "رمضان", "شوال", "ذو القعدة", "ذو الحجة" }[month];
            return new[] { "", "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو",
                "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر" }[month];
        }

        // ==================== FiscalYearPeriodModule (Detail 2) ====================

        public async Task<FiscalYearPeriodModule?> GetModuleByIdAsync(int id)
            => await _db.FiscalYearPeriodModules
                .Include(x => x.FiscalYearPeriod)
                .Include(x => x.Module)
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<FiscalYearPeriodModule>> GetModulesByPeriodIdAsync(int fiscalYearPeriodId)
            => await _db.FiscalYearPeriodModules
                .Where(x => x.FiscalYearPeriodId == fiscalYearPeriodId && x.CancelDate == null)
                .Include(x => x.Module)
                .OrderBy(x => x.ModuleId)
                .AsNoTracking()
                .ToListAsync();

        public async Task<FiscalYearPeriodModule> AddModuleAsync(FiscalYearPeriodModule entity)
        {
            await _db.FiscalYearPeriodModules.AddAsync(entity);
            return entity;
        }

        public Task UpdateModuleAsync(FiscalYearPeriodModule entity)
        {
            _db.FiscalYearPeriodModules.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteModuleAsync(int id)
        {
            var item = await _db.FiscalYearPeriodModules.FindAsync(id);
            if (item != null) _db.FiscalYearPeriodModules.Remove(item);
        }

        public async Task DeleteModulesByPeriodIdAsync(int fiscalYearPeriodId, CancellationToken ct)
        {
            var items = await _db.FiscalYearPeriodModules
                .Where(x => x.FiscalYearPeriodId == fiscalYearPeriodId)
                .ToListAsync(ct);
            _db.FiscalYearPeriodModules.RemoveRange(items);
        }

        public async Task<bool> ModuleExistsAsync(int id)
            => await _db.FiscalYearPeriodModules.AnyAsync(x => x.Id == id);

        public async Task SoftDeleteModuleAsync(int id, int? regUserId = null)
        {
            var item = await _db.FiscalYearPeriodModules.FindAsync(id);
            if (item != null)
            {
                item.Cancel(regUserId);
                _db.FiscalYearPeriodModules.Update(item);
            }
        }

        public async Task<Dictionary<int, List<int>>> GetOpenModulesByFiscalYearIdAsync(int fiscalYearId, CancellationToken ct)
        {
            var result = new Dictionary<int, List<int>>();

            var periods = await _db.FiscalYearPeriods
                .Where(x => x.FiscalYearId == fiscalYearId && x.CancelDate == null)
                .Select(x => x.Id)
                .ToListAsync(ct);

            foreach (var periodId in periods)
            {
                var moduleIds = await _db.FiscalYearPeriodModules
                    .Where(x => x.FiscalYearPeriodId == periodId && x.CancelDate == null && x.OpenDate != null && x.CloseDate == null)
                    .Select(x => x.ModuleId)
                    .ToListAsync(ct);

                if (moduleIds.Any())
                    result[periodId] = moduleIds;
            }

            return result;
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}