using API.Common.Errors;
using VenusHR.Application.Common;
using VenusHR.Core.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using ProblemDetailsFactory = API.Common.Errors.ProblemDetailsFactory;

namespace API.Helpers
{
    public sealed class GlobalExceptionMiddleware : IMiddleware
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

                var pd = new ValidationProblemDetails(errors)
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation failed",
                    Detail = "One or more validation errors occurred.",
                    Instance = context.Request.Path
                };

                pd.Extensions["traceId"] = context.TraceIdentifier;

                await context.Response.WriteAsync(JsonSerializer.Serialize(pd, JsonOptions));
            }
            catch (DomainException ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                _logger.LogWarning(ex, "Domain rule violation. TraceId: {TraceId}", context.TraceIdentifier);

                var pd = ProblemDetailsFactory.Create(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid request",
                    detail: ex.Message,
                    traceId: context.TraceIdentifier,
                    instance: context.Request.Path,
                    type: "https://httpstatuses.com/400"
                );

                await context.Response.WriteAsync(JsonSerializer.Serialize(pd, JsonOptions));
            }
            // ✅ 409 for SQL Server unique constraint / duplicate key violations
            catch (DbUpdateException ex) when (IsSqlServerUniqueViolation(ex))
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status409Conflict;

                _logger.LogWarning(ex, "Unique constraint violation. TraceId: {TraceId}", context.TraceIdentifier);

                var pd = ProblemDetailsFactory.Create(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: "A record with the same unique values already exists.",
                    traceId: context.TraceIdentifier,
                    instance: context.Request.Path,
                    type: "https://httpstatuses.com/409"
                );

                await context.Response.WriteAsync(JsonSerializer.Serialize(pd, JsonOptions));
            }
            catch (NotFoundException ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status404NotFound;

                var pd = ProblemDetailsFactory.Create(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Not found",
                    detail: ex.Message,
                    traceId: context.TraceIdentifier,
                    instance: context.Request.Path,
                    type: "https://httpstatuses.com/404"
                );

                await context.Response.WriteAsync(JsonSerializer.Serialize(pd, JsonOptions));
            }
            catch (ConflictException ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status409Conflict;

                var pd = ProblemDetailsFactory.Create(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: ex.Message,
                    traceId: context.TraceIdentifier,
                    instance: context.Request.Path,
                    type: "https://httpstatuses.com/409"
                );

                await context.Response.WriteAsync(JsonSerializer.Serialize(pd, JsonOptions));
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                _logger.LogError(ex, "Unhandled exception. TraceId: {TraceId}", context.TraceIdentifier);

                var pd = ProblemDetailsFactory.Create(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Server error",
                    detail: "Something went wrong. Please try again later.",
                    traceId: context.TraceIdentifier,
                    instance: context.Request.Path,
                    type: "https://httpstatuses.com/500"
                );

                await context.Response.WriteAsync(JsonSerializer.Serialize(pd, JsonOptions));
            }
        }

        private static bool IsSqlServerUniqueViolation(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
                return sqlEx.Number is 2601 or 2627;

            var inner = ex.InnerException;
            while (inner != null)
            {
                if (inner is SqlException s && (s.Number == 2601 || s.Number == 2627))
                    return true;

                inner = inner.InnerException;
            }

            return false;
        }
    }
}
