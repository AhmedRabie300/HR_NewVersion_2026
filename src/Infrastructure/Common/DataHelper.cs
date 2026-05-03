using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Common.Helpers
{
    public static class DataHelper
    {
        private static string? _connectionString;

        public static void Initialize(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new InvalidOperationException("DataHelper not initialized. Call Initialize() first.");
            return _connectionString;
        }

        public static async Task<string> ExecuteListProcedureAsync(
            string procedureName,
            int? userId,  // ✅ غير من string إلى int?
            int formId,
            string routePath,
            string lang,
            int pageSize,
            int pageNumber,
            string? orderBy = null,
            string? orderDirection = null,
            string? criteria = null)
        {
            using var connection = new SqlConnection(GetConnectionString());
            using var command = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

             command.Parameters.AddWithValue("@User", userId ?? 1);
            command.Parameters.AddWithValue("@FormId", formId);
            command.Parameters.AddWithValue("@RoutePath", routePath);
            command.Parameters.AddWithValue("@Lang", lang);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@PageNumber", pageNumber);

            if (!string.IsNullOrEmpty(orderBy))
                command.Parameters.AddWithValue("@OrderBy", orderBy);

            if (!string.IsNullOrEmpty(orderDirection))
                command.Parameters.AddWithValue("@OrderDirection", orderDirection);

            if (!string.IsNullOrEmpty(criteria))
                command.Parameters.AddWithValue("@Criteria", criteria);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return result?.ToString() ?? "{}";
        }

        public static async Task<T?> ExecuteScalarAsync<T>(string procedureName, params SqlParameter[] parameters)
        {
            using var connection = new SqlConnection(GetConnectionString());
            using var command = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
                return default;

            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static async Task<DataSet> ExecuteDataSetAsync(string procedureName, params SqlParameter[] parameters)
        {
            using var connection = new SqlConnection(GetConnectionString());
            using var command = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            using var adapter = new SqlDataAdapter(command);
            var dataSet = new DataSet();
            await Task.Run(() => adapter.Fill(dataSet));

            return dataSet;
        }

        public static async Task<DataTable> ExecuteDataTableAsync(string procedureName, params SqlParameter[] parameters)
        {
            var dataSet = await ExecuteDataSetAsync(procedureName, parameters);
            return dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
        }

        public static async Task<int> ExecuteNonQueryAsync(string procedureName, params SqlParameter[] parameters)
        {
            using var connection = new SqlConnection(GetConnectionString());
            using var command = new SqlCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync();
        }

        public static string BuildCriteria(params (string key, object value)[] filters)
        {
            var obj = new JObject();
            foreach (var (key, value) in filters)
            {
                if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                    obj[key] = JToken.FromObject(value);
            }
            return obj.ToString();
        }

        public static string BuildDropdownCriteria(string fieldName, int? value, string? text = null)
        {
            if (!value.HasValue || value.Value == 0)
                return "";

            var obj = new JObject();
            obj[fieldName] = new JObject
            {
                ["valueField"] = value.Value,
                ["textField"] = text ?? ""
            };
            return obj.ToString();
        }
    }
}