// Infrastructure/Services/Auth/JwtService.cs
using Application.UARbac.Groups.Dtos;
using Application.UARbac.UserGroups.Dtos;
using Domain.UARbac;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.Auth
{
    public interface IJwtService
    {
        string GenerateToken(Users user, List<UserGroupDto> groups);
        bool ValidateToken(string token);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Users user, List<UserGroupDto> groups)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ??
                throw new ArgumentException("SecretKey not configured"));

            var expiryMinutes = 120;
            if (jwtSettings["ExpiryInMinutes"] != null)
            {
                int.TryParse(jwtSettings["ExpiryInMinutes"], out expiryMinutes);
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id.ToString()),
                new Claim("userCode", user.Code),
                new Claim("userName", user.EngName ?? user.ArbName ?? user.Code),
                new Claim("arabicName", user.ArbName ?? ""),
                new Claim("isAdmin", (user.IsAdmin ?? false).ToString()),
                new Claim("isClient", "false")
            };

            foreach (var group in groups)
            {
                claims.Add(new Claim(ClaimTypes.Role, group.GroupCode));
                claims.Add(new Claim("group", group.GroupCode));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = jwtSettings["Issuer"] ?? "VenusHR",
                Audience = jwtSettings["Audience"] ?? "VenusHRUsers",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? "");

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}