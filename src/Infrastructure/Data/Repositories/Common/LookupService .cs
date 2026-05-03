using Application.Abstractions;
using Application.Common.Abstractions;
using Application.Common.Lookups;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        public async Task<List<LookupDto>> GetLookupAsync<T>(string? criteria = null, CancellationToken ct = default) where T : class
        {
            var companyId = _currentUser.CompanyId;
            var language = _currentUser.Language;
            var dbSet = _db.Set<T>();

            var parameter = Expression.Parameter(typeof(T), "x");

            // Base conditions
            var companyIdProp = Expression.Property(parameter, "CompanyId");
            var companyIdConst = Expression.Constant(companyId);
            var companyIdCondition = Expression.Equal(companyIdProp, companyIdConst);

            var cancelDateProp = Expression.Property(parameter, "CancelDate");
            var cancelDateConst = Expression.Constant(null);
            var cancelDateCondition = Expression.Equal(cancelDateProp, cancelDateConst);

            var baseCondition = Expression.AndAlso(companyIdCondition, cancelDateCondition);

            // Parse criteria
            Expression? criteriaExpression = null;
            if (!string.IsNullOrEmpty(criteria))
            {
                var criteriaObj = JObject.Parse(criteria);
                criteriaExpression = BuildExpression(parameter, typeof(T), criteriaObj);
            }

            var finalCondition = criteriaExpression != null
                ? Expression.AndAlso(baseCondition, criteriaExpression)
                : baseCondition;

            var lambda = Expression.Lambda<Func<T, bool>>(finalCondition, parameter);

            var items = await dbSet
                .Where(lambda)
                .ToListAsync(ct);

            return items.Select(item => new LookupDto(
                Value: (int)item.GetType().GetProperty("Id")?.GetValue(item)!,
                Text: GetTextValue(item, language)
            )).OrderBy(x => x.Text).ToList();
        }

        private Expression? BuildExpression(ParameterExpression parameter, Type entityType, JObject jsonObj)
        {
            Expression? result = null;

            foreach (var prop in jsonObj.Properties())
            {
                var key = prop.Name.ToLower();
                var value = prop.Value;

                if (key == "$and" && value is JArray andArray)
                {
                    foreach (var item in andArray)
                    {
                        var expr = BuildExpression(parameter, entityType, (JObject)item);
                        if (expr != null)
                        {
                            result = result == null ? expr : Expression.AndAlso(result, expr);
                        }
                    }
                }
                else if (key == "$or" && value is JArray orArray)
                {
                    foreach (var item in orArray)
                    {
                        var expr = BuildExpression(parameter, entityType, (JObject)item);
                        if (expr != null)
                        {
                            result = result == null ? expr : Expression.OrElse(result, expr);
                        }
                    }
                }
                else
                {
                    var propertyName = key;
                    var propertyValue = value.ToString();
                    var property = entityType.GetProperty(propertyName, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                    if (property != null)
                    {
                        var condition = BuildCondition(parameter, property, propertyValue);
                        if (condition != null)
                        {
                            result = result == null ? condition : Expression.AndAlso(result, condition);
                        }
                    }
                }
            }

            return result;
        }

        private Expression? BuildCondition(ParameterExpression parameter, PropertyInfo property, string value)
        {
            var propertyExp = Expression.Property(parameter, property);

            if (property.PropertyType == typeof(string))
            {
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var constant = Expression.Constant(value);
                return Expression.Call(propertyExp, containsMethod!, constant);
            }
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                if (int.TryParse(value, out var intValue))
                {
                    var constant = Expression.Constant(intValue);
                    return Expression.Equal(propertyExp, constant);
                }
            }
            else if (property.PropertyType == typeof(short) || property.PropertyType == typeof(short?))
            {
                if (short.TryParse(value, out var shortValue))
                {
                    var constant = Expression.Constant(shortValue);
                    return Expression.Equal(propertyExp, constant);
                }
            }
            else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
            {
                if (bool.TryParse(value, out var boolValue))
                {
                    var constant = Expression.Constant(boolValue);
                    return Expression.Equal(propertyExp, constant);
                }
            }
            else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
            {
                if (decimal.TryParse(value, out var decimalValue))
                {
                    var constant = Expression.Constant(decimalValue);
                    return Expression.Equal(propertyExp, constant);
                }
            }
            else if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
            {
                if (double.TryParse(value, out var doubleValue))
                {
                    var constant = Expression.Constant(doubleValue);
                    return Expression.Equal(propertyExp, constant);
                }
            }

            return null;
        }

        private string GetTextValue(object entity, int language)
        {
            var code = entity.GetType().GetProperty("Code")?.GetValue(entity) as string;
            var engName = entity.GetType().GetProperty("EngName")?.GetValue(entity) as string;
            var arbName = entity.GetType().GetProperty("ArbName")?.GetValue(entity) as string;

            var name = language == 2
                ? (!string.IsNullOrEmpty(arbName) ? arbName : engName ?? "")
                : (!string.IsNullOrEmpty(engName) ? engName : arbName ?? "");

            return string.IsNullOrEmpty(code) ? name : $"{code} - {name}";
        }
    }
}