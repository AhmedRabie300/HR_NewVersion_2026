 namespace VenusHR.Application.Common.DTOs.GroupFeatures;

public class CreateGroupFeatureDto
{
    public int GroupId { get; set; }
    public int FeatureId { get; set; }
    public bool? View { get; set; }
    public bool? Add { get; set; }
    public bool? Edit { get; set; }
    public bool? Delete { get; set; }
    public bool? Export { get; set; }
    public bool? Print { get; set; }
    public string? AllowedItemsJson { get; set; }
    public string? ExcludedItemsJson { get; set; }
}

 

