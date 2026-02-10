using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class UpdateGroupFeatureDto
{
    public bool? CanView { get; set; }
    public bool? CanAdd { get; set; }
    public bool? CanEdit { get; set; }
    public bool? CanDelete { get; set; }
    public bool? CanExport { get; set; }
    public bool? CanPrint { get; set; }
    public string? AllowedItemsJson { get; set; }
    public string? ExcludedItemsJson { get; set; }
}