using Domain.Common;

namespace Domain.System.MasterData
{
    public class Nationality : LegacyEntity,ICompanyScoped
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public bool? IsMainNationality { get; private set; }
        public int? TravelRoute { get; private set; }
        public int? TravelClass { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public double? TicketAmount { get; private set; }
        public int CompanyId { get; private set; }

        // Navigation properties
        // public TicketRoute? TicketRoute { get; private set; } // هتضاف بعدين

        private Nationality() { } // For EF Core

        public Nationality(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            bool? isMainNationality,
            int? travelRoute,
            int? travelClass,
            string? remarks,
            double? ticketAmount)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            IsMainNationality = isMainNationality;
            TravelRoute = travelRoute;
            TravelClass = travelClass;
            Remarks = remarks;
            TicketAmount = ticketAmount;
        }

        // Update methods
        public void UpdateBasicInfo(
            string? engName,
            string? arbName,
            string? arbName4S,
            string? remarks)
        {
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (remarks != null) Remarks = remarks;
        }

        public void UpdateTravelInfo(
            int? travelRoute,
            int? travelClass,
            double? ticketAmount)
        {
            if (travelRoute.HasValue) TravelRoute = travelRoute;
            if (travelClass.HasValue) TravelClass = travelClass;
            if (ticketAmount.HasValue) TicketAmount = ticketAmount;
        }

        public void UpdateNationalityStatus(bool? isMainNationality)
        {
            if (isMainNationality.HasValue)
                IsMainNationality = isMainNationality;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}