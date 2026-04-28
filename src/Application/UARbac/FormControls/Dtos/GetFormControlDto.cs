namespace Application.UARbac.FormControls.Dtos
{
    public sealed record GetFormControlDto(
     int Id,
     int FormId,
     string Name,
     string? FieldName,
     string? Format,
     int? Section,
     string? EngCaption,
     string? ArbCaption,
     bool? Compulsory,
     bool? IsHide,
     bool? IsDisabled,
     int? SearchId,
     int? Rank 
 );

}
