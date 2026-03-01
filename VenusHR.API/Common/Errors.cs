using Microsoft.AspNetCore.Mvc;

namespace API.Common.Errors
{
    public static class ProblemDetailsFactory
    {
        public static ProblemDetails Create(
            int statusCode,
            string title,
            string detail,
            string? traceId = null,
            string? instance = null,
            string? type = null)
        {
            var pd = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = instance,
                Type = type
            };

            if (!string.IsNullOrWhiteSpace(traceId))
                pd.Extensions["traceId"] = traceId;

            return pd;
        }
    }
}
