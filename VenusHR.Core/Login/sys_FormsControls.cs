using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenusHR.Core.Login
{
    [Table("sys_FormsControls")]
    public class sys_FormsControls
    {
        [Key]
        public int ID { get; set; }

        public int FormID { get; set; }

        [MaxLength(200)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? EngCaption { get; set; }

        [MaxLength(500)]
        public string? ArbCaption { get; set; }

        public int? Compulsory { get; set; }

        [MaxLength(100)]
        public string? Format { get; set; }

        [MaxLength(100)]
        public string? ArbFormat { get; set; }

        [MaxLength(1000)]
        public string? ToolTip { get; set; }

        [MaxLength(1000)]
        public string? ArbToolTip { get; set; }

        public int? MaxLength { get; set; }

        public int? IsNumeric { get; set; }

        public int? IsHide { get; set; }

        public int? FocusOnStartUp { get; set; }

        public int? Rank { get; set; }

        public double? MinValue { get; set; }

        public double? MaxValue { get; set; }

        public int? FieldID { get; set; }

        public int? SearchID { get; set; }

        public int? IsArabic { get; set; }

        public int? RegUserID { get; set; }

        [MaxLength(50)]
        public string? RegComputerID { get; set; }

        public DateTime RegDate { get; set; }

        public DateTime? CancelDate { get; set; }

        public string? FieldName { get; set; }
        public int? Section { get; set; }
        public bool? Required { get; set; }
        public bool? Disabled { get; set; }
        public string? ControlType { get; set; }
        [ForeignKey("FormID")]
        public virtual sys_Forms? Form { get; set; }
    }
}