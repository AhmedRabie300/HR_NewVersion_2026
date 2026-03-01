using System;

namespace VenusHR.Application.Common.DTOs.Lookups
{
    public class BloodGroupDto
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string? EngName { get; set; }
        public string? ArbName { get; set; }
        public string? ArbName4S { get; set; }
        public string? Remarks { get; set; }
        public DateTime? CancelDate { get; set; }
        public bool IsActive => !CancelDate.HasValue || CancelDate > DateTime.Now;
    }

    public class CreateBloodGroupDto
    {
        public string Code { get; set; }
        public string? EngName { get; set; }
        public string? ArbName { get; set; }
        public string? ArbName4S { get; set; }
        public string? Remarks { get; set; }
    }

    public class UpdateBloodGroupDto
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string? EngName { get; set; }
        public string? ArbName { get; set; }
        public string? ArbName4S { get; set; }
        public string? Remarks { get; set; }
    }
}