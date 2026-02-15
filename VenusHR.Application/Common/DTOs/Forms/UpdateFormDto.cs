using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace VenusHR.Application.Common.DTOs.Forms
{
    public class UpdateFormDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? EngName { get; set; }
        public string? ArbName { get; set; }
        public string? ArbName4S { get; set; }
        public string? EngDescription { get; set; }
        public string? ArbDescription { get; set; }
        public int? Rank { get; set; }
        public int? ModuleId { get; set; }
        public int? SearchFormId { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public string? Remarks { get; set; }
        public string? Layout { get; set; }
        public string? LinkTarget { get; set; }
        public string? LinkUrl { get; set; }
        public string? ImageUrl { get; set; }
        public int? MainId { get; set; }
    }
}
