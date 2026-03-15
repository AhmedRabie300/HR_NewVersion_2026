using Application.UARbac.Abstractions;
using Application.UARbac.FormPermission.Dtos;
using Application.UARbac.Groups.Dtos;
using Application.UARbac.Login.Dtos;
using Application.UARbac.Menus.Dtos;
using Application.UARbac.UserGroups.Dtos;
using Domain.UARbac;
using Infrastructure.Data;
using Infrastructure.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories.UARbac
{
    public sealed class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;
        private readonly IEncryptionService _encryptionService;
        private readonly IUserGroupRepository _userGroupRepo;
        private readonly IMenuRepository _menuRepo;
        private readonly IFormPermissionRepository _permissionRepo;

        public LoginRepository(
            ApplicationDbContext db,
            IConfiguration configuration,
            IJwtService jwtService,
            IEncryptionService encryptionService,
            IUserGroupRepository userGroupRepo,
            IMenuRepository menuRepo,
            IFormPermissionRepository permissionRepo)
        {
            _db = db;
            _configuration = configuration;
            _jwtService = jwtService;
            _encryptionService = encryptionService;
            _userGroupRepo = userGroupRepo;
            _menuRepo = menuRepo;
            _permissionRepo = permissionRepo;
        }

        public bool ValidateToken(string token)
        {
            return _jwtService.ValidateToken(token);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, int lang, CancellationToken ct)
        {
            try
            {
                // 1. Validate input
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return new LoginResponseDto(
                        false,
                        lang == 1 ? "Please enter username and password" : "برجاء ادخال اسم المستخدم وكلمة المرور",
                        null, null, null, null, null, null, null, null, null, null, null
                    );
                }

                // 2. Encrypt password and find user
                string encryptedPassword = _encryptionService.Encrypt(request.Password, "DataOcean", false);

                var user = await _db.Users
                    .FirstOrDefaultAsync(u =>
                        u.Code == request.Username &&
                        u.Password == encryptedPassword, ct);

                if (user == null)
                {
                    return new LoginResponseDto(
                        false,
                        lang == 1 ? "Invalid username or password" : "اسم المستخدم أو كلمة المرور غير صحيحة",
                        null, null, null, null, null, null, null, null, null, null, null
                    );
                }

                // 3. Check if user is active
                if (!(user.IsActive ?? true))
                {
                    return new LoginResponseDto(
                        false,
                        lang == 1 ? "User account is inactive" : "حساب المستخدم غير مفعل",
                        null, null, null, null, null, null, null, null, null, null, null
                    );
                }

                // 4. Update device token
                if (!string.IsNullOrEmpty(request.DeviceToken))
                {
                    user.UpdateDeviceToken(request.DeviceToken);
                    await _db.SaveChangesAsync(ct);
                }

                // 5. Get user groups
                var userGroups = await _userGroupRepo.GetByUserIdAsync(user.Id);
                var groups = userGroups.Select(ug => new UserGroupDto(
                    ug.Id,
                    ug.Group?.Code ?? "",
                    ug.UserId,
                    ug.User?.Code ?? "",
                    ug.User?.EngName ?? ug.User?.ArbName,
                    ug.GroupId,
                    ug.Group?.Code ?? "",
                    ug.Group?.EngName ?? ug.Group?.ArbName
              
                )).ToList();

                //// 6. Get menus
                //var menus = await _menuRepo.GetAllAsync();
                //var menuDtos = menus.Select(m => new MenuDto(
                //    m.Id,
                //    m.Code,
                //    m.EngName,
                //    m.ArbName,
                //    m.ArbName4S,
                //    m.ParentId,
                //    m.Shortcut,
                //    m.Rank,
                //    m.FormId,
                //    m.Image,
                //    m.ViewType,
                //    m.IsHide != 1,
                //    null
                //)).ToList();
                // 6. Get menus - النسخة النهائية الصحيحة
                List<MenuDto> menuDtos = new List<MenuDto>();
                try
                {
                    var menusData = await _db.Menus
                        .Select(m => new {
                            m.Id,
                            m.Code,
                            m.EngName,
                            m.ArbName,
                            m.ArbName4S,
                        })
                        .ToListAsync();

                    menuDtos = menusData.Select(m => new MenuDto(
                        m.Id,
                        m.Code,
                        m.EngName,
                        m.ArbName,
                        m.ArbName4S,
                     
                        null
                    )).ToList();

                    Console.WriteLine($"Menus loaded successfully: {menuDtos.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Menu error: {ex.Message}");
                    menuDtos = new List<MenuDto>(); // قائمة فاضية عشان اللوجين يكمل
                }
                // 7. Get permissions
                var permissions = await _permissionRepo.GetUserEffectivePermissionsAsync(user.Id);
                var permissionDtos = permissions
                    .GroupBy(x => x.FormId)
                    .Select(g => new UserFormPermissionDto(
                        g.Key,
                        g.First().Form?.Code ?? "",
                        g.First().Form?.EngName ?? g.First().Form?.ArbName ?? "",
                        g.Any(x => x.AllowView == true),
                        g.Any(x => x.AllowAdd == true),
                        g.Any(x => x.AllowEdit == true),
                        g.Any(x => x.AllowDelete == true),
                        g.Any(x => x.AllowPrint == true),
                        g.Count() > 1 ? "Multiple" : (g.First().UserId.HasValue ? "User" : "Group")
                    ))
                    .ToList();

                // 8. Generate token
                var token = _jwtService.GenerateToken(user, groups);

                // 9. Return success response (Employees مش موجودة حالياً)
                return new LoginResponseDto(
                    true,
                    "Login successful",
                    user.Id,
                    user.Code,
                    user.EngName,
                    user.ArbName,
                    token,
                    request.DeviceToken,
                    groups,
                    menuDtos,
                    permissionDtos,
                    null, // EmployeeId
                    null  // EmployeeName
                );
            }
            catch (Exception)
            {
                return new LoginResponseDto(
                    false,
                    lang == 1 ? "Login failed" : "فشل تسجيل الدخول",
                    null, null, null, null, null, null, null, null, null, null, null
                );
            }
        }
    }
}