using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UARbac.Users.Dtos
{
    public class UserGroupInfoDto
    {
        public int GroupId { get; set; }
        public string GroupCode { get; set; } = null!;
        public string GroupName { get; set; } = null!;
        public bool IsPrimary { get; set; }
    }
}
