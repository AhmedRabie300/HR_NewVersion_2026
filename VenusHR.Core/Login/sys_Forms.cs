using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenusHR.Core.Login
{
    [Table("sys_Forms")]
    public class sys_Forms
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

        [MaxLength(500)]
        public string? EngDescription { get; set; }

        [MaxLength(500)]
        public string? ArbDescription { get; set; }

        public int? Rank { get; set; }

        public int ModuleID { get; set; }

        public int? SearchFormID { get; set; }

        public int? Height { get; set; }

        public int? Width { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        public int? RegUserID { get; set; }

        [MaxLength(50)]
        public string? RegComputerID { get; set; }

        public DateTime RegDate { get; set; }

        public DateTime? CancelDate { get; set; }

        [MaxLength(500)]
        public string? Layout { get; set; }

        [MaxLength(100)]
        public string? LinkTarget { get; set; }

        [MaxLength(500)]
        public string? LinkUrl { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public int? MainID { get; set; }

         [ForeignKey("ModuleID")]
        public virtual Sys_Modules? Module { get; set; }

        [ForeignKey("SearchFormID")]
        public virtual sys_Forms? SearchForm { get; set; }

        [ForeignKey("MainID")]
        public virtual sys_Forms? MainForm { get; set; }

        public virtual ICollection<sys_FormsPermissions>? FormsPermissions { get; set; }
    }
}