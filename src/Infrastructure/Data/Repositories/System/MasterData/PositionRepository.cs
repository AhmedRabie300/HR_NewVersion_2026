// Infrastructure/Data/Repositories/System/MasterData/PositionRepository.cs
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Domain.System.MasterData;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.System.MasterData
{
    public sealed class PositionRepository : IPositionRepository
    {
        private readonly ApplicationDbContext _db;

        public PositionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Position?> GetByIdAsync(int id)
        {
            return await _db.Positions
                .Include(x => x.ParentPosition)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Position?> GetByCodeAsync(string code)
        {
            return await _db.Positions
                .Include(x => x.ParentPosition)
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<List<Position>> GetAllAsync()
        {
            return await _db.Positions
                .Where(x => x.CancelDate == null)
                .Include(x => x.ParentPosition)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Position>> GetByParentIdAsync(int parentId)
        {
            return await _db.Positions
                .Where(x => x.CancelDate == null && x.ParentId == parentId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Position>> GetByPositionLevelIdAsync(int positionLevelId)
        {
            return await _db.Positions
                .Where(x => x.CancelDate == null && x.PositionLevelId == positionLevelId)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Position> AddAsync(Position position)
        {
            await _db.Positions.AddAsync(position);
            return position;
        }

        public Task UpdateAsync(Position position)
        {
            _db.Positions.Update(position);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var position = await _db.Positions.FindAsync(id);
            if (position != null)
            {
                _db.Positions.Remove(position);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Positions.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _db.Positions.AnyAsync(x => x.Code == code);
        }

        public async Task<bool> CodeExistsAsync(string code, int excludeId)
        {
            return await _db.Positions
                .AnyAsync(x => x.Code == code && x.Id != excludeId);
        }

        public async Task<List<Position>> GetActivePositionsAsync()
        {
            return await _db.Positions
                .Where(x => x.CancelDate == null)
                .Include(x => x.ParentPosition)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedResult<Position>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            IQueryable<Position> query = _db.Positions
                .Where(x => x.CancelDate == null)
                .Include(x => x.ParentPosition)
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

            return new PagedResult<Position>(items, pageNumber, pageSize, totalCount);
        }

        public async Task SoftDeleteAsync(int id, int? regUserId = null)
        {
            var position = await _db.Positions.FindAsync(id);
            if (position != null)
            {
                position.Cancel(regUserId);
                _db.Positions.Update(position);
            }
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }


        public async Task<string?> GetMaxCodeAsync(int companyId, CancellationToken ct)
        {
            var allCodes = await _db.Positions
                .Where(x =>  x.CancelDate == null)
                .Select(x => x.Code)
                .ToListAsync(ct);

            if (!allCodes.Any())
                return null;

            var maxCode = allCodes
               .Select(code => new
               {
                   Code = code,
                   Number = ExtractNumberFromCode(code)
               })
               .Where(x => x.Number > 0)
               .OrderByDescending(x => x.Number)
               .FirstOrDefault();

            return maxCode?.Code;
        }

        private int ExtractNumberFromCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return 0;

            var match = Regex.Match(code, @"\d+$");
            if (match.Success && int.TryParse(match.Value, out int number))
                return number;

            if (int.TryParse(code, out int directNumber))
                return directNumber;

            return 0;
        }
        public async Task<bool> IsEngNameUniqueAsync(string engName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(engName))
                return true;

            var query = _db.Positions
                .Where(x => x.CancelDate == null
                    && x.EngName != null
                    && x.EngName.Trim().ToLower() == engName.Trim().ToLower());

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return !await query.AnyAsync(ct);
        }

        public async Task<bool> IsArbNameUniqueAsync(string arbName, int? excludeId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(arbName))
                return true;

            var query = _db.Positions
                .Where(x => x.CancelDate == null
                    && x.ArbName != null
                    && x.ArbName.Trim() == arbName.Trim());
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);


            return !await query.AnyAsync(ct);
        }
    }
}
