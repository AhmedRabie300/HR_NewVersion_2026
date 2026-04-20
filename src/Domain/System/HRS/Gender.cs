using Domain.Common;

namespace Domain.System.HRS
{
    public class Gender : LegacyEntity,ICompanyScoped
    {
        public string? Code { get; private set; }
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public int? RegUserId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int CompanyId { get; private set; }

        private Gender() { }

        public Gender(
            string? code,
            string? engName,
            string? arbName,
            int? regUserId)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            RegUserId = regUserId;
            RegDate = DateTime.Now;
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
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}