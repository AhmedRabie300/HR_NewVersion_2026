using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UARbac.Login.Dtos
{
    public class UserLoginResponseDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public LoginResponseDto? Data { get; set; }
    }
}
