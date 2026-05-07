using Application.Abstractions;
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Domain.System.HRS.Basics.Contracts;
using Infrastructure.Common.Helpers;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.HRS
{
    public sealed class ContractRepository : IContractRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUser _currentUser;

        public ContractRepository(ApplicationDbContext db, ICurrentUser currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        // ==================== Contract (Master) ====================

        public async Task<Domain.System.HRS.Basics.Contracts.Contract?> GetByIdAsync(int id)
            => await _db.Contracts
                .Include(x => x.Company)
                .Include(x => x.ContractType)
                .Include(x => x.EmployeeClass)
                .Include(x => x.Employee)
                .Include(x => x.Profession)
                .Include(x => x.Position)
                .Include(x => x.GradeStep)
                .Include(x => x.Currency)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Domain.System.HRS.Basics.Contracts.Contract?> GetByNumberAsync(int number)
            => await _db.Contracts
                .Include(x => x.Company)
                .Include(x => x.ContractType)
                .Include(x => x.EmployeeClass)
                .Include(x => x.Employee)
                .Include(x => x.Profession)
                .Include(x => x.Position)
                .Include(x => x.GradeStep)
                .Include(x => x.Currency)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Number == number);

        public async Task<List<Domain.System.HRS.Basics.Contracts.Contract>> GetAllAsync()
        {
            var companyId = _currentUser.CompanyId;
            return await _db.Contracts
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .Include(x => x.ContractType)
                .Include(x => x.Employee)
                .Include(x => x.Transactions)
                .OrderByDescending(x => x.Number)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Domain.System.HRS.Basics.Contracts.Contract>> GetByCompanyIdAsync()
        {
            var companyId = _currentUser.CompanyId;
            return await _db.Contracts
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .Include(x => x.ContractType)
                .Include(x => x.Employee)
                .Include(x => x.Transactions)
                .OrderByDescending(x => x.Number)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Domain.System.HRS.Basics.Contracts.Contract>> GetByEmployeeIdAsync(int employeeId)
        {
            var companyId = _currentUser.CompanyId;
            return await _db.Contracts
                .Where(x => x.CancelDate == null && x.CompanyId == companyId && x.EmployeeId == employeeId)
                .Include(x => x.Company)
                .Include(x => x.ContractType)
                .Include(x => x.Employee)
                .Include(x => x.Transactions)
                .OrderByDescending(x => x.StartDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Domain.System.HRS.Basics.Contracts.Contract> AddAsync(Domain.System.HRS.Basics.Contracts.Contract entity)
        {
            await _db.Contracts.AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(Domain.System.HRS.Basics.Contracts.Contract entity)
        {
            _db.Contracts.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Contracts.FindAsync(id);
            if (item != null) _db.Contracts.Remove(item);
        }

        public async Task<bool> ExistsAsync(int id)
            => await _db.Contracts.AnyAsync(x => x.Id == id);

        public async Task<bool> NumberExistsAsync(int number)
            => await _db.Contracts.AnyAsync(x => x.Number == number && x.CancelDate == null);

        public async Task<bool> NumberExistsAsync(int number, int excludeId)
            => await _db.Contracts.AnyAsync(x => x.Number == number && x.CancelDate == null && x.Id != excludeId);

        public async Task<PagedResult<Domain.System.HRS.Basics.Contracts.Contract>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var companyId = _currentUser.CompanyId;
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Domain.System.HRS.Basics.Contracts.Contract> query = _db.Contracts
                .Where(x => x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.Company)
                .Include(x => x.ContractType)
                .Include(x => x.Employee)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                if (int.TryParse(searchTerm, out var number))
                {
                    query = query.Where(x => x.Number == number);
                }
                else
                {
                    query = query.Where(x =>
                        (x.Remarks != null && x.Remarks.Contains(searchTerm)) ||
                        (x.ContractType != null && (x.ContractType.EngName != null && x.ContractType.EngName.Contains(searchTerm))) ||
                        (x.Employee != null && (x.Employee.EngName != null && x.Employee.EngName.Contains(searchTerm))));
                }
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(x => x.Number)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Domain.System.HRS.Basics.Contracts.Contract>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id )
        {
            var item = await _db.Contracts.FindAsync(id);
            if (item != null)
            {
                item.Cancel();
                _db.Contracts.Update(item);
            }
        }

        public async Task<int> GetNextNumberAsync(int companyId, CancellationToken ct)
        {
            var maxNumber = await _db.Contracts
                .Where(x => x.CompanyId == companyId && x.CancelDate == null)
                .MaxAsync(x => (int?)x.Number, ct) ?? 0;
            return maxNumber + 1;
        }

        // ==================== ContractTransaction (Detail) ====================

        public async Task<ContractTransaction?> GetTransactionByIdAsync(int id)
            => await _db.ContractTransactions
                .Include(x => x.Contract)
                .Include(x => x.TransactionType)
                .Include(x => x.Interval)
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<ContractTransaction>> GetTransactionsByContractIdAsync(int contractId)
        {
            var companyId = _currentUser.CompanyId;
            return await _db.ContractTransactions
                .Where(x => x.ContractId == contractId && x.CancelDate == null && x.CompanyId == companyId)
                .Include(x => x.TransactionType)
                .Include(x => x.Interval)
                .OrderBy(x => x.Id)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ContractTransaction> AddTransactionAsync(ContractTransaction entity)
        {
            await _db.ContractTransactions.AddAsync(entity);
            return entity;
        }

        public Task UpdateTransactionAsync(ContractTransaction entity)
        {
            _db.ContractTransactions.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var item = await _db.ContractTransactions.FindAsync(id);
            if (item != null) _db.ContractTransactions.Remove(item);
        }

        public async Task<bool> TransactionExistsAsync(int id)
            => await _db.ContractTransactions.AnyAsync(x => x.Id == id);

        public async Task SoftDeleteTransactionAsync(int id )
        {
            var item = await _db.ContractTransactions.FindAsync(id);
            if (item != null)
            {
                item.Cancel();
                _db.ContractTransactions.Update(item);
            }
        }
        public async Task<string?> GetListJsonAsync(int pageNumber, int pageSize, string? orderBy, string? orderDirection, string? criteria)
        {
            var userId = _currentUser.UserId ?? 1;
            var lang = _currentUser.Language == 2 ? "AR" : "EN";
            var companyId = _currentUser.CompanyId;

            // Add companyId to criteria
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
                "hrs_ContractsGetList",
                userId,
                0,
                "/basics/contracts",
                lang,
                pageSize,
                pageNumber,
                orderBy,
                orderDirection,
                criteria);
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}