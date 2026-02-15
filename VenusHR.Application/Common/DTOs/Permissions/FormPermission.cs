using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenusHR.Application.Common.DTOs.Permissions
 {
    public class FormPermissionDto
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public int? GroupId { get; set; }
        public int? UserId { get; set; }
        public bool AllowView { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowPrint { get; set; }
        public int? RegUserId { get; set; }
        public string? RegComputerId { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime? CancelDate { get; set; }

         public FormDto? Form { get; set; }
    }
}