 namespace VenusHR.Application.Common.DTOs.GroupFeatures;

public class CreateGroupFeatureDto
{
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
}

 

