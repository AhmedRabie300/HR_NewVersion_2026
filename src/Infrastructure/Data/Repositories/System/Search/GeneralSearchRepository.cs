using Application.System.Search.Abstractions;
using Application.System.Search.Dtos;
using Application.Common.Abstractions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Data.Repositories.System.Search
{
    public sealed class GeneralSearchRepository : IGeneralSearchRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IContextService _contextService;

        public GeneralSearchRepository(ApplicationDbContext db, IContextService contextService)
        {
            _db = db;
            _contextService = contextService;
        }

        public async Task<List<SearchColumnResponseDto>> GetSearchColumnsAsync(int searchID, int lang, CancellationToken ct)
        {
            var columns = await _db.Sys_SearchsColumns
                .Include(x => x.Field)
                .Where(x => x.SearchID == searchID && x.CancelDate == null)
                .OrderBy(x => x.RankCriteria)
                .ThenBy(x => x.RankView)
                .ToListAsync(ct);

            return columns.Select(c => new SearchColumnResponseDto(
                FieldID: c.FieldID,
                FieldName: c.Field?.FieldName ?? "",
                DisplayName: lang == 2 ? (c.ArbName ?? c.EngName ?? c.Field?.FieldName ?? "") : (c.EngName ?? c.ArbName ?? c.Field?.FieldName ?? ""),
                FieldType: c.Field?.FieldType?.ToString(),
                FieldLength: c.Field?.FieldLength,
                IsCriteria: c.IsCriteria ?? false,
                IsView: c.IsView ?? false,
                RankCriteria: c.RankCriteria,
                RankView: c.RankView
            )).ToList();
        }

        public async Task<SearchExecuteResultDto> ExecuteSearchAsync(SearchExecuteRequestDto request, int lang, int companyId, CancellationToken ct)
        {
            try
            {
                 var search = await _db.Sys_Searchs
                    .Include(x => x.Object)
                    .FirstOrDefaultAsync(x => x.Id == request.SearchID && x.CancelDate == null, ct);

                if (search == null)
                {
                    return new SearchExecuteResultDto(request.SearchID, "", 0, request.PageNumber, request.PageSize, new List<Dictionary<string, object>>());
                }

                 var allColumns = await GetSearchColumnsAsync(request.SearchID, lang, ct);
                var viewColumns = allColumns.Where(x => x.IsView).ToList();
                var criteriaColumns = allColumns.Where(x => x.IsCriteria).ToList();

                 var tableName =  search.Object.Code;

                 var (sqlQuery, parameters) = BuildDynamicSqlQuery(
                    tableName,
                    search.Object?.Code ?? "",
                    request.SearchTerm,
                    criteriaColumns,
                    viewColumns,
                    companyId,
                    request.UserId,
                    request.AnotherCriteria
                );

                 if (string.IsNullOrEmpty(sqlQuery))
                {
                    return new SearchExecuteResultDto(request.SearchID, "", 0, request.PageNumber, request.PageSize, new List<Dictionary<string, object>>());
                }

                 var totalCount = await GetTotalCountAsync(sqlQuery, parameters, ct);
                var pagedSqlQuery = BuildPagedSqlQuery(sqlQuery, request, parameters);
                var items = await ExecuteQueryAsync(pagedSqlQuery, parameters, ct);

                 var translatedItems = items.Select(item =>
                {
                    var translatedItem = new Dictionary<string, object>();
                    foreach (var kv in item)
                    {
                        var column = viewColumns.FirstOrDefault(c => c.FieldName == kv.Key);
                        if (column != null)
                        {
                            var displayKey = lang == 2 ? (column.DisplayName) : column.FieldName;
                            translatedItem[displayKey] = kv.Value;
                        }
                        else
                        {
                            translatedItem[kv.Key] = kv.Value;
                        }
                    }
                    return translatedItem;
                }).ToList();

                return new SearchExecuteResultDto(
                    request.SearchID,
                    lang == 2 ? (search.ArbName ?? search.EngName ?? "") : (search.EngName ?? search.ArbName ?? ""),
                    totalCount,
                    request.PageNumber,
                    request.PageSize,
                    translatedItems
                );
            }
            catch (Exception ex)
            {
                return new SearchExecuteResultDto(request.SearchID, "", 0, request.PageNumber, request.PageSize, new List<Dictionary<string, object>>());
            }
        }

        #region Helper Methods

        
        private (string SqlQuery, Dictionary<string, object> Parameters) BuildDynamicSqlQuery(
            string tableName,
            string objectCode,
            string searchTerm,
            List<SearchColumnResponseDto> criteriaColumns,
            List<SearchColumnResponseDto> viewColumns,
            int companyId,
            int? userId,
            string? anotherCriteria)
        {
            var parameters = new Dictionary<string, object>();
            var whereConditions = new List<string>();
            var selectFields = new List<string>();

             foreach (var col in viewColumns)
            {
                if (!selectFields.Contains(col.FieldName))
                    selectFields.Add(col.FieldName);
            }

             whereConditions.Add("CancelDate IS NULL");

             if (companyId > 0 && objectCode.ToLower() != "company")
            {
                whereConditions.Add("CompanyID = @CompanyId");
                parameters["@CompanyId"] = companyId;
            }

             // if (userId.HasValue && userId.Value > 0)
            // {
            //     whereConditions.Add("UserID = @UserId");
            //     parameters["@UserId"] = userId.Value;
            // }

             if (!string.IsNullOrWhiteSpace(anotherCriteria))
            {
                whereConditions.Add(anotherCriteria);
            }

             if (!string.IsNullOrWhiteSpace(searchTerm) && criteriaColumns.Any())
            {
                var searchConditions = new List<string>();
                foreach (var col in criteriaColumns)
                {
                    searchConditions.Add($"{col.FieldName} LIKE @SearchTerm");
                }
                whereConditions.Add($"({string.Join(" OR ", searchConditions)})");
                parameters["@SearchTerm"] = $"%{EscapeSqlString(searchTerm)}%";
            }

            var selectClause = string.Join(", ", selectFields);
            var whereClause = string.Join(" AND ", whereConditions);
            var sqlQuery = $"SELECT {selectClause} FROM {tableName} WHERE {whereClause}";

            return (sqlQuery, parameters);
        }

       
        private string BuildPagedSqlQuery(string baseSqlQuery, SearchExecuteRequestDto request, Dictionary<string, object> parameters)
        {
            var sortBy = string.IsNullOrEmpty(request.SortBy) ? "Code" : request.SortBy;
            var sortDirection = request.SortDescending ? "DESC" : "ASC";
            var offset = request.PageSize * (request.PageNumber - 1);
            var fetch = request.PageSize;

            return $"{baseSqlQuery} ORDER BY {sortBy} {sortDirection} OFFSET {offset} ROWS FETCH NEXT {fetch} ROWS ONLY";
        }

        private async Task<int> GetTotalCountAsync(string sqlQuery, Dictionary<string, object> parameters, CancellationToken ct)
        {
            var countQuery = $"SELECT COUNT(1) FROM ({sqlQuery}) AS CountQuery";
            return await ExecuteScalarAsync<int>(countQuery, parameters, ct);
        }

        private async Task<List<Dictionary<string, object>>> ExecuteQueryAsync(string sqlQuery, Dictionary<string, object> parameters, CancellationToken ct)
        {
            var result = new List<Dictionary<string, object>>();

            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlQuery;
                command.CommandType = CommandType.Text;

                foreach (var param in parameters)
                {
                    var dbParam = command.CreateParameter();
                    dbParam.ParameterName = param.Key;
                    dbParam.Value = param.Value ?? DBNull.Value;
                    command.Parameters.Add(dbParam);
                }

                if (command.Connection?.State != ConnectionState.Open)
                    await command.Connection!.OpenAsync(ct);

                using (var reader = await command.ExecuteReaderAsync(ct))
                {
                    while (await reader.ReadAsync(ct))
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.GetValue(i);
                        }
                        result.Add(row);
                    }
                }
            }

            return result;
        }

        private async Task<T> ExecuteScalarAsync<T>(string sqlQuery, Dictionary<string, object> parameters, CancellationToken ct)
        {
            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlQuery;
                command.CommandType = CommandType.Text;

                foreach (var param in parameters)
                {
                    var dbParam = command.CreateParameter();
                    dbParam.ParameterName = param.Key;
                    dbParam.Value = param.Value ?? DBNull.Value;
                    command.Parameters.Add(dbParam);
                }

                if (command.Connection?.State != ConnectionState.Open)
                    await command.Connection!.OpenAsync(ct);

                var result = await command.ExecuteScalarAsync(ct);
                return result != DBNull.Value ? (T)Convert.ChangeType(result, typeof(T)) : default!;
            }
        }

        private string EscapeSqlString(string value)
        {
            return value.Replace("'", "''");
        }

        #endregion
    }
}