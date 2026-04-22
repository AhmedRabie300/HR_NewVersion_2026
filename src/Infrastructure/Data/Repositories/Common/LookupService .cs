using Application.Abstractions;
using Application.Common.Abstractions;
using Application.Common.Lookups;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Services.Common
{
    public sealed class LookupService : ILookupService
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUser _currentUser;

        public LookupService(ApplicationDbContext db, ICurrentUser currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<List<LookupDto>> GetLookupAsync<T>(CancellationToken ct = default) where T : class
        {
            var companyId = _currentUser.CompanyId;
            var language = _currentUser.Language;  
            var dbSet = _db.Set<T>();

           
            var parameter = Expression.Parameter(typeof(T), "x");

             var companyIdProp = Expression.Property(parameter, "CompanyId");
            var companyIdConst = Expression.Constant(companyId);
            var companyIdCondition = Expression.Equal(companyIdProp, companyIdConst);

             var cancelDateProp = Expression.Property(parameter, "CancelDate");
            var cancelDateConst = Expression.Constant(null);
            var cancelDateCondition = Expression.Equal(cancelDateProp, cancelDateConst);

             var finalCondition = Expression.AndAlso(companyIdCondition, cancelDateCondition);
            var lambda = Expression.Lambda<Func<T, bool>>(finalCondition, parameter);

             var items = await dbSet
                .Where(lambda)
                .ToListAsync(ct);

             return items.Select(item => new LookupDto(
                Value: (int)item.GetType().GetProperty("Id")?.GetValue(item)!,
                Text: GetTextValue(item, language)
            )).OrderBy(x => x.Text).ToList();
        }

        private string GetTextValue(object entity, int language)
        {
            var engName = entity.GetType().GetProperty("EngName")?.GetValue(entity) as string;
            var arbName = entity.GetType().GetProperty("ArbName")?.GetValue(entity) as string;

            if (language == 2)  
                return !string.IsNullOrEmpty(arbName) ? arbName : engName ?? "";

             return !string.IsNullOrEmpty(engName) ? engName : arbName ?? "";
        }
    }
}