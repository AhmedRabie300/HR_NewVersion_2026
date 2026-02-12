using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GroupFeatureDto
{
    public int GroupId { get; set; }
    public string GroupCode { get; set; } = null!;
    public string GroupName { get; set; } = null!;
    public int FeatureId { get; set; }
    public string? FeatureArabicName { get; set; }
    public string? FeatureEnglishName { get; set; }
    public bool? View { get; set; }
    public bool? Add { get; set; }
    public bool? Edit { get; set; }
    public bool? Delete { get; set; }
    public bool? Export { get; set; }
    public bool? Print { get; set; }
    public List<string> AllowedItems { get; set; } = new();
    public List<string> ExcludedItems { get; set; } = new();
}