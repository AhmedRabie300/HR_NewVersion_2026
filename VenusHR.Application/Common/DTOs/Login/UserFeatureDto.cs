using System.Text.Json.Serialization;

namespace VenusHR.Application.Common.DTOs.Login;

public class UserFeatureDto
{
    public int FeatureId { get; set; }
    public string FeatureName { get; set; } = null!;
    public string? ArabicName { get; set; }
    public string? EnglishName { get; set; }
    public int ModuleId { get; set; }

     public bool CanView { get; set; }
    public bool CanAdd { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanExport { get; set; }
    public bool CanPrint { get; set; }
    public bool Hidden { get; set; }

     public List<string> AllowedItems { get; set; } = new();
    public List<string> ExcludedItems { get; set; } = new();

     public List<string> SourceGroups { get; set; } = new();
}