using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VenusHR.Core.Login;

public class Sys_GroupFeatures
{
    [Key]
    public int ID { get; set; }
    public int GroupId { get; set; }
     public int FeatureId { get; set; }

     public bool? CanView { get; set; }
    public bool? CanAdd { get; set; }
    public bool? CanEdit { get; set; }
    public bool? CanDelete { get; set; }
    public bool? CanExport { get; set; }
    public bool? CanPrint { get; set; }

    public string? AllowedItemsJson { get; set; }  
    public string? ExcludedItemsJson { get; set; } 
    public int? RegUserId { get; set; }
    public DateTime RegDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

     public virtual Sys_Groups Group { get; set; } = null!;
    public virtual Sys_Features Feature { get; set; } = null!;
}
 