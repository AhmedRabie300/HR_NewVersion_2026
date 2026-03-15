using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UARbac.FormControls.Dtos
{
    public sealed record UpdateFormControlDto(
     int Id,
     string? EngCaption,
     string? ArbCaption,
     bool? IsHide,
     bool? IsDisabled
 );

}
