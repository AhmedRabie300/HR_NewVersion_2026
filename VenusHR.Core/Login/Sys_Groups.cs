using System;
using System.Collections.Generic;

namespace VenusHR.Core.Login;

public class Sys_Groups
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

 
    public string? ArbName { get; set; }

    public string? EngName { get; set; }

 
 
    public int? RegUserId { get; set; }

    public DateTime RegDate { get; set; }

    public DateTime? CancelDate { get; set; }

     public virtual ICollection<Sys_GroupsUsers> GroupUsers { get; set; } = new List<Sys_GroupsUsers>();
    public virtual ICollection<Sys_GroupFeatures> GroupFeatures { get; set; } = new List<Sys_GroupFeatures>();
}