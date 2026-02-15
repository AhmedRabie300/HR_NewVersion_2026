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
    [Table("sys_FormsPermissions")]
    public class sys_FormsPermissions
    {
        [Key]
        public int ID { get; set; }

        public int FormID { get; set; }

        public int? GroupID { get; set; }

        public int? UserID { get; set; }

        public bool? AllowView { get; set; }

        public bool? AllowAdd { get; set; }

        public bool? AllowEdit { get; set; }

        public bool? AllowDelete { get; set; }

        public bool? AllowPrint { get; set; }

        public int? RegUserID { get; set; }

        [MaxLength(50)]
        public string? RegComputerID { get; set; }

        public DateTime RegDate { get; set; }

        public DateTime? CancelDate { get; set; }

 
        [ForeignKey("FormID")]
        public virtual sys_Forms? Form { get; set; }

        [ForeignKey("GroupID")]
        public virtual Sys_Groups? Group { get; set; }

        [ForeignKey("UserID")]
        public virtual Sys_Users? User { get; set; }
    }
}