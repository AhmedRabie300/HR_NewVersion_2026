using Application.Abstractions;
using Application.Common.Abstractions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.Repositories.Common
{
    public sealed class CodeGeneratorService : ICodeGeneratorService
    {
        private readonly ApplicationDbContext _db;
        private readonly ICurrentUser _currentUser;

        public CodeGeneratorService(ApplicationDbContext db, ICurrentUser currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public async Task<string> GetNextCodeAsync<T>(CancellationToken ct = default) where T : class
        {
            var companyId = _currentUser.CompanyId;
            var dbSet = _db.Set<T>();

            // بناء الـ Expression ديناميكياً
            var parameter = Expression.Parameter(typeof(T), "x");

            // شرط CompanyId
            var companyIdProp = Expression.Property(parameter, "CompanyId");
            var companyIdConst = Expression.Constant(companyId);
            var companyIdCondition = Expression.Equal(companyIdProp, companyIdConst);

            // شرط Code != null
            var codeProp = Expression.Property(parameter, "Code");
            var notNullCondition = Expression.NotEqual(codeProp, Expression.Constant(null));

            // دمج الشرطين
            var finalCondition = Expression.AndAlso(companyIdCondition, notNullCondition);
            var lambda = Expression.Lambda<Func<T, bool>>(finalCondition, parameter);

            // جلب كل الأكواد
            var allCodes = await dbSet
                .Where(lambda)
                .Select(x => EF.Property<string>(x, "Code"))
                .ToListAsync(ct);

            if (!allCodes.Any())
                return "1";

            // استخراج أكبر رقم
            var maxNumber = allCodes
                .Select(ExtractNumberFromCode)
                .Where(n => n > 0)
                .DefaultIfEmpty(0)
                .Max();

            return (maxNumber + 1).ToString();
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
    }
}