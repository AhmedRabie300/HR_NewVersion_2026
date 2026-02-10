using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenusHR.Application.Common.DTOs.Users
{
    public class UserGroupInfoDto
    {
        public int GroupId { get; set; }
        public string GroupCode { get; set; } = null!;
        public string GroupName { get; set; } = null!;
        public bool IsPrimary { get; set; }
    }
}
