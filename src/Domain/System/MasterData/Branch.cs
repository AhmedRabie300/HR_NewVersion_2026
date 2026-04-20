using Domain.Common;

namespace Domain.System.MasterData
{
    public class Branch : LegacyEntity, ICompanyScoped
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public int? ParentId { get; private set; }
        public int CompanyId { get; private set; }
        public int? CountryId { get; private set; }
        public int? CityId { get; private set; }
        public bool? DefaultAbsent { get; private set; }
        public int? PrepareDay { get; private set; }
        public bool? AffectPeriod { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }
        public Branch? ParentBranch { get; private set; }
        public ICollection<Branch>? ChildBranches { get; private set; }
        // public City? City { get; private set; } // هتضاف بعدين
        // public Country? Country { get; private set; } // هتضاف بعدين

        private Branch() { } // For EF Core

        public Branch(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            int? parentId,
            int? countryId,
            int? cityId,
            bool? defaultAbsent,
            int? prepareDay,
            bool? affectPeriod,
            string? remarks)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            ParentId = parentId;
            CountryId = countryId;
            CityId = cityId;
            DefaultAbsent = defaultAbsent;
            PrepareDay = prepareDay;
            AffectPeriod = affectPeriod;
            Remarks = remarks;
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

        public void UpdateLocation(
            int? countryId,
            int? cityId)
        {
            if (countryId.HasValue) CountryId = countryId;
            if (cityId.HasValue) CityId = cityId;
        }

        public void UpdateParent(int? parentId)
        {
            if (parentId.HasValue) ParentId = parentId;
        }

        public void UpdateSettings(
            bool? defaultAbsent,
            int? prepareDay,
            bool? affectPeriod)
        {
            if (defaultAbsent.HasValue) DefaultAbsent = defaultAbsent;
            if (prepareDay.HasValue) PrepareDay = prepareDay;
            if (affectPeriod.HasValue) AffectPeriod = affectPeriod;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}