using System;
using System.Collections.Generic;

namespace VenusHR.Core.Login;

public class Sys_Features
{
    public int ID { get; set; }

     public string? ArabicName { get; set; }

    public string? EnglishName { get; set; }

    public int ModuleID { get; set; } 


    public bool? IsActive { get; set; }

    public int? RegUserId { get; set; }

    public DateTime RegDate { get; set; }

    public DateTime? CancelDate { get; set; }


    public virtual ICollection<Sys_GroupFeatures> GroupFeatures { get; set; } = new List<Sys_GroupFeatures>();
}