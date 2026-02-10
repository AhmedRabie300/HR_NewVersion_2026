using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenusHR.Application.Common.DTOs.Features
{


    public class FeatureDto
    {
        public int Id { get; set; }
        public string? ArabicName { get; set; }
        public string? EnglishName { get; set; }
        public string? ModuleID { get; set; }
        public bool? IsActive { get; set; }
        public int GroupCount { get; set; }
    }
}
