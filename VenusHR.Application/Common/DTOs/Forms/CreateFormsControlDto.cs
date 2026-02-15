using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenusHR.Application.Common.DTOs.Forms
{
    public class CreateFormsControlDto
    {
        public int FormId { get; set; }
        public string? Name { get; set; }
        public string? EngCaption { get; set; }
        public string? ArbCaption { get; set; }
        public bool? Compulsory { get; set; }
        public string? Format { get; set; }
        public string? ArbFormat { get; set; }
        public string? ToolTip { get; set; }
        public string? ArbToolTip { get; set; }
        public int? MaxLength { get; set; }
        public bool? IsNumeric { get; set; }
        public bool? IsHide { get; set; }
        public bool? FocusOnStartUp { get; set; }
        public int? Rank { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public int? FieldId { get; set; }
        public int? SearchId { get; set; }
        public bool? IsArabic { get; set; }
    }
}