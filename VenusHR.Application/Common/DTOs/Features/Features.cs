 namespace VenusHR.Application.Common.DTOs.Features;

public class CreateFeatureDto
{
    public string? ArabicName { get; set; }
    public string? EnglishName { get; set; }
    public string? ModuleID { get; set; }
    public bool? IsActive { get; set; } = true;
}
