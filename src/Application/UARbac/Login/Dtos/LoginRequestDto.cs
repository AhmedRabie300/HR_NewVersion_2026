using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UARbac.Login.Dtos
{
    public class LoginRequestDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? DeviceToken { get; set; }
    }
}
