using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class UpdateGroupFeatureDto
{
    public bool? View { get; set; }
    public bool? Add { get; set; }
    public bool? Edit { get; set; }
    public bool? Delete { get; set; }
    public bool? Export { get; set; }
    public bool? Print { get; set; }
    public string? AllowedItemsJson { get; set; }
    public string? ExcludedItemsJson { get; set; }
}