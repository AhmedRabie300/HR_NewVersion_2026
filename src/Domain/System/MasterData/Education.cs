using Domain.Common;

namespace Domain.System.MasterData
{
    public class Education : LegacyEntity
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int? Level { get; private set; }
        public double? RequiredYears { get; private set; }
        public int CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        public Company? Company { get; private set; }

        private Education() { }

        public Education(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int? level,
            double? requiredYears,
            string? remarks)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            Level = level;
            RequiredYears = requiredYears;
            Remarks = remarks;
        }

        public void Update(
            string? engName,
            string? arbName,
            string? arbName4S,
            int? level,
            double? requiredYears,
            string? remarks)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (level.HasValue) Level = level;
            if (requiredYears.HasValue) RequiredYears = requiredYears;
            if (remarks != null) Remarks = remarks;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}