using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VenusHR.Application.Common.DTOs.Login;
using VenusHR.Application.Common.DTOs.Menus;
using VenusHR.Application.Common.DTOs.Permissions;
using VenusHR.Application.Common.Interfaces.Login;
using VenusHR.Application.Common.Interfaces.Menus;
using VenusHR.Application.Common.Interfaces.Permissions;
using VenusHR.Application.Common.Interfaces.Users;
using VenusHR.Core.Login;
using WorkFlow_EF;

namespace VenusHR.Infrastructure.Presistence.Login
{
    public class LoginServices : ILoginServices
    {
        private readonly ApplicationDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMenuService _menuService;
        private readonly IFormPermissionService _permissionService;
        private GeneralOutputClass<object> _result;

        public LoginServices(
            ApplicationDBContext context,
            IConfiguration configuration,
            IUserService userService,
            IMenuService menuService,
            IFormPermissionService permissionService)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
            _menuService = menuService;
            _permissionService = permissionService;
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

                 var groupsResult = await _userService.GetUserGroupsAsync(user.Id);
                var groups = groupsResult.Success ? groupsResult.Data ?? new() : new();

                 var menus = await _menuService.GetUserMenusAsync(user.Id);
                var permissions = await _permissionService.GetUserPermissionsAsync(user.Id);

                 var token = GenerateJwtToken(user, groups);

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
                    UserName = user.Code,
                    IsAdmin = user.IsAdmin ?? false,
                    IsClient = false,
                    Token = token,
                    DeviceToken = request.DeviceToken,
                    Groups = groups,
                    Menus = menus,
                    FormPermissions = permissions,
                    EmployeeId = employee.id,
                    EmployeeName = employee?.ArbName
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = lang == 1 ? "Login failed" : "فشل تسجيل الدخول";
             }

            return response;
        }

        public object Login(string username, string password, int lang, string deviceToken)
        {
            _result = new GeneralOutputClass<object>();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                _result.ErrorCode = 1;
                _result.ErrorMessage = lang == 1
                    ? "Please enter username and password"
                    : "برجاء ادخال اسم المستخدم و كلمة المرور";
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

        private string GenerateJwtToken(Sys_Users user, List<UserGroupDto> groups)
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
            try
            {
                string text = _Encrypt(sToEncrypt, sPassword, "777777", "SHA1", 2, "GLORY_BE_TO_GOD!", 256);
                if (ReturnOnlyNumbersAndLetters)
                {
                    text = _S2N(text);
                }
                return text;
            }
            catch (Exception)
            {
                return "ERROR";
            }
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
                var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "");

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
}