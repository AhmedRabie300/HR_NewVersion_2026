using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VenusHR.Application.Common.DTOs.Login;
using VenusHR.Application.Common.Interfaces.Login;
using VenusHR.Core.Login;
using WorkFlow_EF;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace VenusHR.Infrastructure.Presistence.Login
{
    public class LoginServices : ILoginServices
    {
        private readonly ApplicationDBContext _context;
        private readonly IConfiguration _configuration;
        private GeneralOutputClass<object> _result;

        public LoginServices(ApplicationDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _result = new GeneralOutputClass<object>();
        }

         public async Task<UserLoginResponseDto> LoginAsync(LoginRequestDto request, int lang)
        {
            var response = new UserLoginResponseDto();

            try
            {
                 if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    response.Success = false;
                    response.Message = lang == 1
                        ? "Please enter username and password"
                        : "برجاء ادخال اسم المستخدم وكلمة المرور";
                    return response;
                }

                 string encryptedPassword = Encrypt(request.Password, "DataOcean", false);
                var user = await _context.Sys_Users
                    .FirstOrDefaultAsync(u =>
                        u.Code == request.Username &&
                        u.Password == encryptedPassword);

                if (user == null)
                {
                    response.Success = false;
                    response.Message = lang == 1
                        ? "Invalid username or password"
                        : "اسم المستخدم أو كلمة المرور غير صحيحة";
                    return response;
                }

                 if (!(user.IsActive ?? true))
                {
                    response.Success = false;
                    response.Message = lang == 1
                        ? "User account is inactive"
                        : "حساب المستخدم غير مفعل";
                    return response;
                }

                 user.DeviceToken = request.DeviceToken;
                await _context.SaveChangesAsync();

                 var userGroups = await GetUserGroupsAsync(user.Id);

                 var userFeatures = await GetUserFeaturesAsync(user.Id, userGroups);

                 var token = GenerateJwtToken(user, userGroups);

                 var employee = await _context.Hrs_Employees
                    .FirstOrDefaultAsync(e => e.Code == user.Code);

                 response.Success = true;
                response.Message = "Login successful";
                response.Data = new UserDataDto
                {
                    Id = user.Id,
                    Code = user.Code,
                    EngName = user.EngName,
                    ArbName = user.ArbName,
                    IsAdmin = user.IsAdmin ?? false,
                    IsClient = false,
                    Token = token,
                    DeviceToken = request.DeviceToken,
                    Groups = userGroups,
                    Features = userFeatures
                    //EmployeeId = employee?.Id,
                    //EmployeeName = employee?.ArbName
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = lang == 1 ? "Login failed" : "فشل تسجيل الدخول";
                 // _logger.LogError(ex, "Login error for user: {Username}", request.Username);
            }

            return response;
        }

         public object Login(string username, string password, int lang, string deviceToken)
        {
             _result = new GeneralOutputClass<object>();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                _result.ErrorCode = 1;
                if (lang == 1)
                {
                    _result.ErrorMessage = "Please enter username and password";
                }
                else
                {
                    _result.ErrorMessage = "برجاء ادخال اسم المستخدم و كلمة المرور";
                }
                return _result;
            }

            try
            {
                string ORGPassword = _context.Sys_Users
                    .Where(U => U.Code == username)
                    .Select(U => U.Password)
                    .FirstOrDefault();

                if (ORGPassword == Encrypt(password, "DataOcean", false))
                {
                    var user = _context.Sys_Users.FirstOrDefault(U => U.Code == username);
                    if (user != null)
                    {
                         user.DeviceToken = deviceToken;
                        _context.SaveChanges();
                    }

                    _result.ErrorCode = 1;
                    _result.ErrorMessage = "Success";

                     var employee = _context.Hrs_Employees
                        .FirstOrDefault(F => F.Code == username);

                    _result.ResultObject = employee;
                }
                else
                {
                    _result.ErrorCode = 0;
                    _result.ErrorMessage = lang == 1
                        ? "Invalid username or password"
                        : "اسم المستخدم أو كلمة المرور غير صحيحة";
                }
            }
            catch (Exception ex)
            {
                _result.ErrorCode = 0;
                _result.ErrorMessage = lang == 1 ? "Login failed" : "فشل تسجيل الدخول";
            }

            return _result;
        }

 
         private async Task<List<UserGroupDto>> GetUserGroupsAsync(int userId)
        {
            try
            {
                var query = from gu in _context.Sys_GroupsUsers
                            join g in _context.Sys_Groups on gu.GroupId equals g.Id
                            where gu.UserId == userId
                            select new UserGroupDto
                            {
                                Id = g.Id,
                                Code = g.Code,
                                 ArbName = g.ArbName,
                                EngName = g.EngName
                                
                            };

                return await query.ToListAsync();

                
            }
            catch (Exception)
            {
                return new List<UserGroupDto>();
            }
        }

         private async Task<List<UserFeatureDto>> GetUserFeaturesAsync(int userId, List<UserGroupDto> userGroups)
        {
            try
            {
                if (!userGroups.Any())
                    return new List<UserFeatureDto>();

                var groupIds = userGroups.Select(g => g.Id).ToList();

                 var groupFeatures = await _context.Sys_GroupFeatures
                    .Where(gf => groupIds.Contains(gf.GroupId))
                    .Include(gf => gf.Feature)
                    .Include(gf => gf.Group)
                    .ToListAsync();

                 var featuresDict = new Dictionary<int, UserFeatureDto>();

                foreach (var gf in groupFeatures)
                {
                    if (gf.Feature == null || !(gf.Feature.IsActive ?? true))
                        continue;

                    var featureId = gf.Feature.ID;

                    if (!featuresDict.ContainsKey(featureId))
                    {
                        featuresDict[featureId] = new UserFeatureDto
                        {
                            FeatureId = featureId,
                            FeatureName = gf.Feature.EnglishName ?? gf.Feature.ArabicName ?? "Unknown",
                            ArabicName = gf.Feature.ArabicName,
                            EnglishName = gf.Feature.EnglishName,
                            ModuleId = gf.Feature.ModuleID,
                            Hidden = false
                        };
                    }

                    var feature = featuresDict[featureId];

                     feature.CanView |= gf.CanView ?? false;
                    feature.CanAdd |= gf.CanAdd ?? false;
                    feature.CanEdit |= gf.CanEdit ?? false;
                    feature.CanDelete |= gf.CanDelete ?? false;
                    feature.CanExport |= gf.CanExport ?? false;
                    feature.CanPrint |= gf.CanPrint ?? false;

                     if (!string.IsNullOrEmpty(gf.AllowedItemsJson))
                    {
                        try
                        {
                            var items = JsonSerializer.Deserialize<List<string>>(gf.AllowedItemsJson);
                            if (items != null)
                            {
                                feature.AllowedItems = feature.AllowedItems
                                    .Union(items)
                                    .Distinct()
                                    .ToList();
                            }
                        }
                        catch {  }
                    }

                     if (!string.IsNullOrEmpty(gf.ExcludedItemsJson))
                    {
                        try
                        {
                            var items = JsonSerializer.Deserialize<List<string>>(gf.ExcludedItemsJson);
                            if (items != null)
                            {
                                feature.ExcludedItems = feature.ExcludedItems
                                    .Union(items)
                                    .Distinct()
                                    .ToList();
                            }
                        }
                        catch {    }
                    }

                     if (!feature.SourceGroups.Contains(gf.Group.Code))
                    {
                        feature.SourceGroups.Add(gf.Group.Code);
                    }
                }

                return featuresDict.Values.ToList();
            }
            catch (Exception)
            {
                return new List<UserFeatureDto>();
            }
        }

         private string GenerateJwtToken(Sys_Users user, List<UserGroupDto> groups)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

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
                claims.Add(new Claim(ClaimTypes.Role, group.Code));
                claims.Add(new Claim("group", group.Code));
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

 
        public string Encrypt(string sToEncrypt, string sPassword, bool ReturnOnlyNumbersAndLetters)
        {
            string result;
            try
            {
                string text = _Encrypt(sToEncrypt, sPassword, "777777", "SHA1", 2, "GLORY_BE_TO_GOD!", 256);
                if (ReturnOnlyNumbersAndLetters)
                {
                    text = _S2N(text);
                }
                result = text;
            }
            catch (Exception ex)
            {
                result = "ERROR";
            }
            return result;
        }

        private string _Encrypt(string plainText, string passPhrase, string saltValue, string hashAlgorithm,
                              int passwordIterations, string initVector, int keySize)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            var passwordDeriveBytes = new PasswordDeriveBytes(passPhrase,
                Encoding.ASCII.GetBytes(saltValue), hashAlgorithm, passwordIterations);

            using (var rijndaelManaged = new RijndaelManaged())

            {
                rijndaelManaged.Mode = CipherMode.CBC;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream,
                        rijndaelManaged.CreateEncryptor(
                            passwordDeriveBytes.GetBytes(keySize / 8),
                            Encoding.ASCII.GetBytes(initVector)),
                        CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        private string _S2N(string text)
        {
            return new string(text.Where(c => char.IsLetterOrDigit(c)).ToArray());
        }

 
        public bool ValidateToken(string token)
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

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

         public async Task<UserLoginResponseDto> RefreshToken(string refreshToken, int lang)
        {
            var response = new UserLoginResponseDto();

            try
            {
           
                response.Success = false;
                response.Message = "Refresh token not implemented yet";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = lang == 1 ? "Token refresh failed" : "فشل تجديد التوكن";
            }

            return response;
        }
    }

 
    //public class UserDataDto
    //{
    //    public int Id { get; set; }
    //    public string Code { get; set; } = null!;
    //    public string? EngName { get; set; }
    //    public string? ArbName { get; set; }
    //    public bool IsAdmin { get; set; }
    //    public bool IsClient { get; set; }
    //    public string Token { get; set; } = null!;
    //    public string? DeviceToken { get; set; }
    //    public List<UserGroupDto> Groups { get; set; } = new();
    //    public List<UserFeatureDto> Features { get; set; } = new();
    //    public int? EmployeeId { get; set; }
    //    public string? EmployeeName { get; set; }
    //}
}