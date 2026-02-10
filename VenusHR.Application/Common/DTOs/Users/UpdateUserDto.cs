using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenusHR.Application.Common.DTOs.Users
{
    public class UpdateUserDto
    {
        public string? EngName { get; set; }
        public string? ArbName { get; set; }
        public bool? IsAdmin { get; set; }
        public string? DeviceToken { get; set; }
        public List<int>? GroupIds { get; set; }
    }

}
