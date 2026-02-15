 using VenusHR.Application.Common.DTOs;
using VenusHR.Application.Common.DTOs.Menus;
using VenusHR.Application.Common.DTOs.Permissions;

namespace VenusHR.Application.Common.DTOs.Login
{
    public class UserDataDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? EngName { get; set; }
        public string? ArbName { get; set; }
        public string UserName { get; set; } = string.Empty;   
        public bool IsAdmin { get; set; }
        public bool IsClient { get; set; }
        public string Token { get; set; } = string.Empty;
        public string? DeviceToken { get; set; }
        public List<UserGroupDto> Groups { get; set; } = new();

         public List<MenuDto> Menus { get; set; } = new();
        public List<FormPermissionDto> FormPermissions { get; set; } = new();

         public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
    }
}