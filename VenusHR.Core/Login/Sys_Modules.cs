using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenusHR.Core.Login
{
    [Table("Sys_Modules")]
    public class Sys_Modules
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string? Code { get; set; }

        [MaxLength(100)]
        public string? Prefix { get; set; }

        [MaxLength(200)]
        public string? EngName { get; set; }

        [MaxLength(200)]
        public string? ArbName { get; set; }

        [MaxLength(200)]
        public string? ArbName4S { get; set; }

        public int? FormID { get; set; }

        public bool? IsRegistered { get; set; }

        public bool? FiscalYearDependant { get; set; }

        public int? Rank { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        public bool? ISAR { get; set; }

        public bool? ISAP { get; set; }

        public bool? IsGL { get; set; }

        public bool? ISFA { get; set; }

        public bool? IsINV { get; set; }

        public bool? IsHR { get; set; }

        public bool? ISMANF { get; set; }

        public bool? IsSYS { get; set; }

        public int? RegUserID { get; set; }

        [MaxLength(50)]
        public string? RegComputerID { get; set; }

        public DateTime RegDate { get; set; }

        public DateTime? CancelDate { get; set; }

 
   
    }
}
