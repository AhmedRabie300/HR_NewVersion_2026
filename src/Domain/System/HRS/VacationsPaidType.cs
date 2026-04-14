using Domain.Common;

namespace Domain.System.HRS
{
    public class VacationsPaidType : LegacyEntity
    {
        public string? Code { get; private set; }
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public int? RegUserId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        private VacationsPaidType() { }

        public VacationsPaidType(
            string? code,
            string? engName,
            string? arbName,
            int? regUserId)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            RegUserId = regUserId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? code,
            string? engName,
            string? arbName)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.UtcNow;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}