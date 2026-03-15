using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UARbac.Users.Dtos
{
    public sealed record UpdateUserDto(
        int Id,
        string? EngName,
        string? ArbName,
        bool? IsAdmin,
        bool? IsActive,
        string? DeviceToken,
         List<int>? GroupIds
        );
   
     
}
