using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenusHR.Core.Login
{
    [Table("sys_Menus")]
    public class sys_Menus
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string? Code { get; set; }

        [MaxLength(200)]
        public string? EngName { get; set; }

        [MaxLength(200)]
        public string? ArbName { get; set; }

        [MaxLength(200)]
        public string? ArbName4S { get; set; }

        public int? ParentID { get; set; }

        [MaxLength(50)]
        public string? Shortcut { get; set; }

        public int? Rank { get; set; }

        public int? FormID { get; set; }

        public int? ObjectID { get; set; }

        public int? ViewFormID { get; set; }

        public int? IsHide { get; set; }

        [MaxLength(500)]
        public string? Image { get; set; }

        public int? ViewType { get; set; }

        public int? RegUserID { get; set; }

        [MaxLength(50)]
        public string? RegComputerID { get; set; }

        public DateTime RegDate { get; set; }

        public DateTime? CancelDate { get; set; }

 
        [ForeignKey("ParentID")]
        public virtual sys_Menus? Parent { get; set; }

        [ForeignKey("FormID")]
        public virtual sys_Forms? Form { get; set; }

        [ForeignKey("ViewFormID")]
        public virtual sys_Forms? ViewForm { get; set; }

        public virtual ICollection<sys_Menus>? Children { get; set; }
    }
}