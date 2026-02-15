using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenusHR.Application.Common.DTOs.Menus
{
    public class UpdateMenuDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? EngName { get; set; }
        public string? ArbName { get; set; }
        public string? ArbName4S { get; set; }
        public int? ParentId { get; set; }
        public string? Shortcut { get; set; }
        public int? Rank { get; set; }
        public int? FormId { get; set; }
        public int? ObjectId { get; set; }
        public int? ViewFormId { get; set; }
        public bool? IsHide { get; set; }
        public string? Image { get; set; }
        public int? ViewType { get; set; }
    }
}
