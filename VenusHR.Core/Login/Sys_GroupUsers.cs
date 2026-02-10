using System;
using System.ComponentModel.DataAnnotations;

namespace VenusHR.Core.Login;

public class Sys_GroupsUsers
{
    [Key] public int Id { get; set; }
     public int UserId { get; set; }
    public int GroupId { get; set; }

    public bool? IsPrimary { get; set; }

    public DateTime JoinedDate { get; set; }

    public int? RegUserId { get; set; }
    public DateTime RegDate { get; set; }

   }

 