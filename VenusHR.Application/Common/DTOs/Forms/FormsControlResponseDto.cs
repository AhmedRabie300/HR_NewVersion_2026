using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenusHR.Application.Common.DTOs.Forms
 
{
    public class FormsControlResponseDto
    {
        public int FormId { get; set; }
        public List<FormControlDetailDto> FormControls { get; set; } = new();
    }

    public class FormControlDetailDto
    {
        public string? ControlName { get; set; }
        public string? FieldName { get; set; }
        public string? ArbCaption { get; set; }
        public string? EngCaption { get; set; }
        public bool? IsHidden { get; set; }
        public bool? Required { get; set; }
        public string? ControlType { get; set; }
        public int? SearchID { get; set; }
        public string? AnotherCriteria { get; set; }
        public bool? Disabled { get; set; }
        public int? Width { get; set; }
        public int? SectionID { get; set; }
        public string? EngToolTip { get; set; }
        public string? ArbToolTip { get; set; }
    }
}
