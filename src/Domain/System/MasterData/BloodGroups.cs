using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.System.MasterData
{
    public class BloodGroups : LegacyEntity
    {
            public string Code { get; set; }
            public string? EngName { get; set; }
            public string? ArbName { get; set; }
            public string? ArbName4S { get; set; }
            public string? Remarks { get; set; }
            public int RegComputerID { get; set; }
            public DateTime CancelDate { get; set; }
        
    }
}
