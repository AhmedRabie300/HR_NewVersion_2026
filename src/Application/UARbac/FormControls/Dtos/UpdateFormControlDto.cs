using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UARbac.FormControls.Dtos
{
    public sealed record UpdateFormControlDto(
     int Id,
     string? EngCaption,
     string? ArbCaption,
     bool? IsCompulory,
     bool? IsHide,
     bool? IsDisabled
 );

}
