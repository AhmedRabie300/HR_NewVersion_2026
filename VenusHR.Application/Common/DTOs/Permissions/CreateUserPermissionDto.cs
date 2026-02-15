using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenusHR.Application.Common.DTOs.Permissions
{
    public class CreateUserPermissionDto
    {
        public int UserId { get; set; }
        public int FormId { get; set; }
        public bool AllowView { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowPrint { get; set; }
    }
}