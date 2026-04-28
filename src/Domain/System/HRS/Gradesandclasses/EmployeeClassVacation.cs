using Domain.Common;
using Domain.System.HRS.Basics.GradesAndClasses;
using Domain.System.HRS.VacationAndEndOfService;

namespace Domain.System.HRS.Basics.GradesAndClasses
{
    public class EmployeeClassVacation : LegacyEntity, ICompanyScoped
    {
        public int EmployeeClassId { get; private set; }
        public int VacationTypeId { get; private set; }
        public int DurationDays { get; private set; }
        public int? RequiredWorkingMonths { get; private set; }
        public float? FromMonth { get; private set; }
        public float? ToMonth { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? TicketsRnd { get; private set; }
        public int? DependantTicketRnd { get; private set; }
        public int? MaxKeepDays { get; private set; }
        public int CompanyId { get; private set; }

        // Navigation properties
        public EmployeeClass? EmployeeClass { get; private set; }
        public VacationsType? VacationType { get; private set; }

        private EmployeeClassVacation() { }

        public EmployeeClassVacation(
            int employeeClassId,
            int vacationTypeId,
            int durationDays,
            int? requiredWorkingMonths,
            float? fromMonth,
            float? toMonth,
            int companyId,
            string? remarks,
            int? ticketsRnd,
            int? dependantTicketRnd,
            int? maxKeepDays,
            int? regComputerId = null)
        {
            EmployeeClassId = employeeClassId;
            VacationTypeId = vacationTypeId;
            DurationDays = durationDays;
            RequiredWorkingMonths = requiredWorkingMonths;
            FromMonth = fromMonth;
            ToMonth = toMonth;
            CompanyId = companyId;
            Remarks = remarks;
            TicketsRnd = ticketsRnd;
            DependantTicketRnd = dependantTicketRnd;
            MaxKeepDays = maxKeepDays;
            RegComputerId = regComputerId;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            int? durationDays,
            int? requiredWorkingMonths,
            float? fromMonth,
            float? toMonth,
            string? remarks,
            int? ticketsRnd,
            int? dependantTicketRnd,
            int? maxKeepDays)
        {
            if (durationDays.HasValue) DurationDays = durationDays.Value;
            if (requiredWorkingMonths.HasValue) RequiredWorkingMonths = requiredWorkingMonths.Value;
            if (fromMonth.HasValue) FromMonth = fromMonth.Value;
            if (toMonth.HasValue) ToMonth = toMonth.Value;
            if (remarks != null) Remarks = remarks;
            if (ticketsRnd.HasValue) TicketsRnd = ticketsRnd.Value;
            if (dependantTicketRnd.HasValue) DependantTicketRnd = dependantTicketRnd.Value;
            if (maxKeepDays.HasValue) MaxKeepDays = maxKeepDays.Value;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}