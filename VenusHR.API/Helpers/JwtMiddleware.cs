using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VenusHR.API.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly HashSet<string> _publicEndpoints;

        public JwtMiddleware(
            RequestDelegate next,
            IOptions<JwtSettings> jwtSettings,
            ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;

            _publicEndpoints = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "/api/auth/login",
                "/api/hr-master/health",
                "/swagger",
                "/favicon.ico",
                "/swagger/v1/swagger.json",
                "/swagger/index.html"
            };
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                _logger.LogDebug($"AllowAnonymous endpoint: {path}");
                await _next(context);
                return;
            }
            // Skip public endpoints
            if (IsPublicEndpoint(path))
            {
                _logger.LogDebug($"Public endpoint: {path}");
                await _next(context);
                return;
            }

             var token = ExtractTokenFromHeader(context.Request.Headers);

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning($"No token provided for: {path}");
                await ReturnUnauthorizedResponse(context, "Authorization token is required");
                return;
            }

             var validationResult = await ValidateToken(token);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"Invalid token for: {path}. Error: {validationResult.ErrorMessage}");
                await ReturnUnauthorizedResponse(context, validationResult.ErrorMessage ?? "Invalid token");
                return;
            }

             AttachUserToContext(context, validationResult.Principal!);

             _logger.LogInformation($"User authenticated - ID: {context.Items["UserId"]}, Code: {context.Items["UserCode"]}");

            await _next(context);
        }

        private bool IsPublicEndpoint(string path)
        {
            return _publicEndpoints.Any(endpoint =>
                path.StartsWith(endpoint, StringComparison.OrdinalIgnoreCase));
        }

        private string? ExtractTokenFromHeader(IHeaderDictionary headers)
        {
            var authHeader = headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader))
                return null;

            if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return authHeader["Bearer ".Length..].Trim();

            return null;
        }

        private async Task<(bool IsValid, ClaimsPrincipal? Principal, string? ErrorMessage)> ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                if (!tokenHandler.CanReadToken(token))
                    return (false, null, "Invalid token format");

                var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5),
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                return (true, principal, null);
            }
            catch (SecurityTokenExpiredException)
            {
                return (false, null, "Token has expired");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return (false, null, "Invalid token signature");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token validation error");
                return (false, null, $"Token validation failed: {ex.Message}");
            }
        }
         private void AttachUserToContext(HttpContext context, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                var userId = claimsPrincipal.FindFirst("userId")?.Value;
                var userCode = claimsPrincipal.FindFirst("userCode")?.Value;
                var userName = claimsPrincipal.FindFirst("userName")?.Value;
                var arabicName = claimsPrincipal.FindFirst("arabicName")?.Value;
                var isAdmin = claimsPrincipal.FindFirst("isAdmin")?.Value;
                var isClient = claimsPrincipal.FindFirst("isClient")?.Value;
                var role = claimsPrincipal.FindFirst("role")?.Value;
                var group = claimsPrincipal.FindFirst("group")?.Value;

                 var featuresClaim = claimsPrincipal.FindFirst("features")?.Value;
                if (!string.IsNullOrEmpty(featuresClaim))
                {
                    context.Items["UserFeatures"] = featuresClaim;
                }

                context.Items["UserId"] = userId;
                context.Items["UserCode"] = userCode;
                context.Items["UserName"] = userName;
                context.Items["ArabicName"] = arabicName;
                context.Items["IsAdmin"] = isAdmin;
                context.Items["IsClient"] = isClient;
                context.Items["Role"] = role;
                context.Items["Group"] = group;
                context.Items["IsAuthenticated"] = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error attaching user to context");
            }
        }
        private async Task ReturnUnauthorizedResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message = message,
                timestamp = DateTime.UtcNow,
                path = context.Request.Path.Value
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenExpiryMinutes { get; set; } = 20;
    }
}