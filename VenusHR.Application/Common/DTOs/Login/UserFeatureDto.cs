using System.Text.Json.Serialization;

namespace VenusHR.Application.Common.DTOs.Login;

public class UserFeatureDto
{
    public int FeatureId { get; set; }
    public string FeatureName { get; set; } = null!;
    public string? ArabicName { get; set; }
    public string? EnglishName { get; set; }
    public int ModuleId { get; set; }

     public bool View { get; set; }
    public bool Add { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public bool Export { get; set; }
    public bool Print { get; set; }
    public bool Hidden { get; set; }

     public List<string> AllowedItems { get; set; } = new();
    public List<string> ExcludedItems { get; set; } = new();

     public List<string> SourceGroups { get; set; } = new();
}