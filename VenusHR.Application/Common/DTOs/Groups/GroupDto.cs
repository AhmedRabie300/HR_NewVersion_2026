using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenusHR.Application.Common.DTOs.Groups
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ArabicName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int UserCount { get; set; }
        public int FeatureCount { get; set; }
    }
}
