using Domain.Common;

namespace Domain.System.MasterData
{
    public class Project : LegacyEntity, ICompanyScoped
    {
        public string Code { get; private set; } = null!;
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public string? Phone { get; private set; }
        public string? Mobile { get; private set; }
        public string? Fax { get; private set; }
        public string? Email { get; private set; }
        public string? Adress { get; private set; }
        public string? ContactPerson { get; private set; }
        public int? ProjectPeriod { get; private set; }
        public int? ClaimDuration { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public decimal? CreditLimit { get; private set; }
        public int? CreditPeriod { get; private set; }
        public bool? IsAdvance { get; private set; }
        public bool? IsHijri { get; private set; }
        public int? NotifyPeriod { get; private set; }
        public string? CompanyConditions { get; private set; }
        public string? ClientConditions { get; private set; }
        public bool? IsLocked { get; private set; }
        public bool? IsStoped { get; private set; }
        public int? BranchId { get; private set; }
        public int CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int? RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public string? WorkConditions { get; private set; }
        public int? LocationId { get; private set; }
        public int? AbsentTransaction { get; private set; }
        public int? LeaveTransaction { get; private set; }
        public int? LateTransaction { get; private set; }
        public int? SickTransaction { get; private set; }
        public int? OTTransaction { get; private set; }
        public int? HOTTransaction { get; private set; }
        public string? CostCenterCode1 { get; private set; }
        public int? DepartmentId { get; private set; }
        public string? CostCenterCode2 { get; private set; }
        public string? CostCenterCode3 { get; private set; }
        public string? CostCenterCode4 { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }
        public Branch? Branch { get; private set; }
        public Department? Department { get; private set; }
        public Location? Location { get; private set; }

        private Project() { }

        public Project(
            string code,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? phone,
            string? mobile,
            string? fax,
            string? email,
            string? adress,
            string? contactPerson,
            int? projectPeriod,
            int? claimDuration,
            DateTime? startDate,
            DateTime? endDate,
            decimal? creditLimit,
            int? creditPeriod,
            bool? isAdvance,
            bool? isHijri,
            int? notifyPeriod,
            string? companyConditions,
            string? clientConditions,
            bool? isLocked,
            bool? isStoped,
            int? branchId,
            string? remarks,
            string? workConditions,
            int? locationId,
            int? absentTransaction,
            int? leaveTransaction,
            int? lateTransaction,
            int? sickTransaction,
            int? oTTransaction,
            int? hOTTransaction,
            string? costCenterCode1,
            int? departmentId,
            string? costCenterCode2,
            string? costCenterCode3,
            string? costCenterCode4)
        {
            Code = code;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            Phone = phone;
            Mobile = mobile;
            Fax = fax;
            Email = email;
            Adress = adress;
            ContactPerson = contactPerson;
            ProjectPeriod = projectPeriod;
            ClaimDuration = claimDuration;
            StartDate = startDate;
            EndDate = endDate;
            CreditLimit = creditLimit;
            CreditPeriod = creditPeriod;
            IsAdvance = isAdvance;
            IsHijri = isHijri;
            NotifyPeriod = notifyPeriod;
            CompanyConditions = companyConditions;
            ClientConditions = clientConditions;
            IsLocked = isLocked;
            IsStoped = isStoped;
            BranchId = branchId;
            Remarks = remarks;
            WorkConditions = workConditions;
            LocationId = locationId;
            AbsentTransaction = absentTransaction;
            LeaveTransaction = leaveTransaction;
            LateTransaction = lateTransaction;
            SickTransaction = sickTransaction;
            OTTransaction = oTTransaction;
            HOTTransaction = hOTTransaction;
            CostCenterCode1 = costCenterCode1;
            DepartmentId = departmentId;
            CostCenterCode2 = costCenterCode2;
            CostCenterCode3 = costCenterCode3;
            CostCenterCode4 = costCenterCode4;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? code,
            string? engName,
            string? arbName,
            string? arbName4S,
            string? phone,
            string? mobile,
            string? fax,
            string? email,
            string? adress,
            string? contactPerson,
            int? projectPeriod,
            int? claimDuration,
            DateTime? startDate,
            DateTime? endDate,
            decimal? creditLimit,
            int? creditPeriod,
            bool? isAdvance,
            bool? isHijri,
            int? notifyPeriod,
            string? companyConditions,
            string? clientConditions,
            bool? isLocked,
            bool? isStoped,
            int? branchId,
            string? remarks,
            string? workConditions,
            int? locationId,
            int? absentTransaction,
            int? leaveTransaction,
            int? lateTransaction,
            int? sickTransaction,
            int? oTTransaction,
            int? hOTTransaction,
            string? costCenterCode1,
            int? departmentId,
            string? costCenterCode2,
            string? costCenterCode3,
            string? costCenterCode4)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (phone != null) Phone = phone;
            if (mobile != null) Mobile = mobile;
            if (fax != null) Fax = fax;
            if (email != null) Email = email;
            if (adress != null) Adress = adress;
            if (contactPerson != null) ContactPerson = contactPerson;
            if (projectPeriod.HasValue) ProjectPeriod = projectPeriod;
            if (claimDuration.HasValue) ClaimDuration = claimDuration;
            if (startDate.HasValue) StartDate = startDate;
            if (endDate.HasValue) EndDate = endDate;
            if (creditLimit.HasValue) CreditLimit = creditLimit;
            if (creditPeriod.HasValue) CreditPeriod = creditPeriod;
            if (isAdvance.HasValue) IsAdvance = isAdvance;
            if (isHijri.HasValue) IsHijri = isHijri;
            if (notifyPeriod.HasValue) NotifyPeriod = notifyPeriod;
            if (companyConditions != null) CompanyConditions = companyConditions;
            if (clientConditions != null) ClientConditions = clientConditions;
            if (isLocked.HasValue) IsLocked = isLocked;
            if (isStoped.HasValue) IsStoped = isStoped;
            if (branchId.HasValue) BranchId = branchId;
            if (remarks != null) Remarks = remarks;
            if (workConditions != null) WorkConditions = workConditions;
            if (locationId.HasValue) LocationId = locationId;
            if (absentTransaction.HasValue) AbsentTransaction = absentTransaction;
            if (leaveTransaction.HasValue) LeaveTransaction = leaveTransaction;
            if (lateTransaction.HasValue) LateTransaction = lateTransaction;
            if (sickTransaction.HasValue) SickTransaction = sickTransaction;
            if (oTTransaction.HasValue) OTTransaction = oTTransaction;
            if (hOTTransaction.HasValue) HOTTransaction = hOTTransaction;
            if (costCenterCode1 != null) CostCenterCode1 = costCenterCode1;
            if (departmentId.HasValue) DepartmentId = departmentId;
            if (costCenterCode2 != null) CostCenterCode2 = costCenterCode2;
            if (costCenterCode3 != null) CostCenterCode3 = costCenterCode3;
            if (costCenterCode4 != null) CostCenterCode4 = costCenterCode4;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool IsActive() => !CancelDate.HasValue;
    }
}