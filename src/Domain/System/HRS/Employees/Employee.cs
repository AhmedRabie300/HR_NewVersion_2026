using Domain.Common;
using Domain.System.MasterData;

namespace Domain.System.HRS.Employees
{
    public class Employee : LegacyEntity, ICompanyScoped
    {
        public string Code { get; private set; } = null!;
        public string? OldCode { get; private set; }
        public string? EngName { get; private set; }
        public string? ArbName { get; private set; }
        public string? ArbName4S { get; private set; }
        public string? FamilyEngName { get; private set; }
        public string? FamilyArbName { get; private set; }
        public string? FamilyArbName4S { get; private set; }
        public string? FatherEngName { get; private set; }
        public string? FatherArbName { get; private set; }
        public string? FatherArbName4S { get; private set; }
        public string? GrandEngName { get; private set; }
        public string? GrandArbName { get; private set; }
        public string? GrandArbName4S { get; private set; }
        public DateTime? BirthDate { get; private set; }
        public int? BirthCityId { get; private set; }
        public int? ReligionId { get; private set; }
        public int? MaritalStatusId { get; private set; }
        public string? Sex { get; private set; }
        public int? BloodGroupId { get; private set; }
        public int? BankId { get; private set; }
        public int? NationalityId { get; private set; }
        public string? BankAccountNumber { get; private set; }
        public string? BankAccNumber { get; private set; }
        public int? DepartmentId { get; private set; }
        public string? GOSINumber { get; private set; }
        public DateTime? GOSIJoinDate { get; private set; }
        public DateTime? GOSIExcludeDate { get; private set; }
        public DateTime? JoinDate { get; private set; }
        public DateTime? ExcludeDate { get; private set; }
        public int CompanyId { get; private set; }
        public string? Remarks { get; private set; }
        public int RegUserId { get; private set; }
        public int? RegComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }
        public int? BranchId { get; private set; }
        public int? SponsorId { get; private set; }
        public string? Email { get; private set; }
        public string? Phone { get; private set; }
        public string? Mobile { get; private set; }
        public int? ManagerId { get; private set; }
        public string? MachineCode { get; private set; }
        public int? SectorId { get; private set; }
        public string? SSnNo { get; private set; }
        public string? PassPortNo { get; private set; }
        public string? EntryNo { get; private set; }
        public int? Cost1 { get; private set; }
        public int? Cost2 { get; private set; }
        public int? Cost3 { get; private set; }
        public int? Cost4 { get; private set; }
        public string? LaborOfficeNo { get; private set; }
        public int? LocationId { get; private set; }
        public float? WHours { get; private set; }
        public bool? IsProjectRelated { get; private set; }
        public bool? IsSpecialForce { get; private set; }
        public double? MaxLoanDedution { get; private set; }
        public string? LedgerCode { get; private set; }
        public bool? HasTaqat { get; private set; }
        public string? BankAccountType { get; private set; }
        public bool? Hasflexiblesalarydist { get; private set; }
        public int? Paymenttype { get; private set; }
        public string? WorkEmail { get; private set; }
        public string? SSNOIssueDate { get; private set; }
        public string? SSNOExpireDate { get; private set; }
        public string? PassportIssueDate { get; private set; }
        public string? PassportExpireDate { get; private set; }
        public string? AddressAsPerContract { get; private set; }
        public bool? InsertRequestsForAnotherEmployee { get; private set; }
        public bool? IsSocialInsuranceIncluded { get; private set; }
        public int? UpdateUserId { get; private set; }
        public DateTime? UpdateDate { get; private set; }

        // Navigation properties
        public Company? Company { get; private set; }
        public City? BirthCity { get; private set; }
        public Religion? Religion { get; private set; }
        public MaritalStatus? MaritalStatus { get; private set; }
        public BloodGroup? BloodGroup { get; private set; }
        public Bank? Bank { get; private set; }
        public Nationality? Nationality { get; private set; }
        public Department? Department { get; private set; }
        public Branch? Branch { get; private set; }
        public Sponsor? Sponsor { get; private set; }
        public Sector? Sector { get; private set; }
        public Location? Location { get; private set; }
        public Employee? Manager { get; private set; }

        private Employee() { }

        public Employee(
            string code,
            int companyId,
            int regUserId,
            string? engName = null,
            string? arbName = null,
            string? arbName4S = null,
            string? familyEngName = null,
            string? familyArbName = null,
            string? familyArbName4S = null,
            string? fatherEngName = null,
            string? fatherArbName = null,
            string? fatherArbName4S = null,
            string? grandEngName = null,
            string? grandArbName = null,
            string? grandArbName4S = null,
            DateTime? birthDate = null,
            int? birthCityId = null,
            int? religionId = null,
            int? maritalStatusId = null,
            string? sex = null,
            int? bloodGroupId = null,
            int? bankId = null,
            int? nationalityId = null,
            string? bankAccountNumber = null,
            string? bankAccNumber = null,
            int? departmentId = null,
            string? gosiNumber = null,
            DateTime? gosiJoinDate = null,
            DateTime? gosiExcludeDate = null,
            DateTime? joinDate = null,
            DateTime? excludeDate = null,
            string? remarks = null,
            int? regComputerId = null,
            int? branchId = null,
            int? sponsorId = null,
            string? email = null,
            string? phone = null,
            string? mobile = null,
            int? managerId = null,
            string? machineCode = null,
            int? sectorId = null,
            string? ssnNo = null,
            string? passportNo = null,
            string? entryNo = null,
            int? cost1 = null,
            int? cost2 = null,
            int? cost3 = null,
            int? cost4 = null,
            string? laborOfficeNo = null,
            int? locationId = null,
            float? wHours = null,
            bool? isProjectRelated = null,
            bool? isSpecialForce = null,
            double? maxLoanDedution = null,
            string? ledgerCode = null,
            bool? hasTaqat = null,
            string? bankAccountType = null,
            bool? hasflexiblesalarydist = null,
            int? paymenttype = null,
            string? workEmail = null,
            string? ssnOIssueDate = null,
            string? ssnOExpireDate = null,
            string? passportIssueDate = null,
            string? passportExpireDate = null,
            string? addressAsPerContract = null,
            bool? insertRequestsForAnotherEmployee = null,
            bool? isSocialInsuranceIncluded = null)
        {
            Code = code;
            CompanyId = companyId;
            RegUserId = regUserId;
            EngName = engName;
            ArbName = arbName;
            ArbName4S = arbName4S;
            FamilyEngName = familyEngName;
            FamilyArbName = familyArbName;
            FamilyArbName4S = familyArbName4S;
            FatherEngName = fatherEngName;
            FatherArbName = fatherArbName;
            FatherArbName4S = fatherArbName4S;
            GrandEngName = grandEngName;
            GrandArbName = grandArbName;
            GrandArbName4S = grandArbName4S;
            BirthDate = birthDate;
            BirthCityId = birthCityId;
            ReligionId = religionId;
            MaritalStatusId = maritalStatusId;
            Sex = sex;
            BloodGroupId = bloodGroupId;
            BankId = bankId;
            NationalityId = nationalityId;
            BankAccountNumber = bankAccountNumber;
            BankAccNumber = bankAccNumber;
            DepartmentId = departmentId;
            GOSINumber = gosiNumber;
            GOSIJoinDate = gosiJoinDate;
            GOSIExcludeDate = gosiExcludeDate;
            JoinDate = joinDate;
            ExcludeDate = excludeDate;
            Remarks = remarks;
            RegComputerId = regComputerId;
            BranchId = branchId;
            SponsorId = sponsorId;
            Email = email;
            Phone = phone;
            Mobile = mobile;
            ManagerId = managerId;
            MachineCode = machineCode;
            SectorId = sectorId;
            SSnNo = ssnNo;
            PassPortNo = passportNo;
            EntryNo = entryNo;
            Cost1 = cost1;
            Cost2 = cost2;
            Cost3 = cost3;
            Cost4 = cost4;
            LaborOfficeNo = laborOfficeNo;
            LocationId = locationId;
            WHours = wHours;
            IsProjectRelated = isProjectRelated;
            IsSpecialForce = isSpecialForce;
            MaxLoanDedution = maxLoanDedution;
            LedgerCode = ledgerCode;
            HasTaqat = hasTaqat;
            BankAccountType = bankAccountType;
            Hasflexiblesalarydist = hasflexiblesalarydist;
            Paymenttype = paymenttype;
            WorkEmail = workEmail;
            SSNOIssueDate = ssnOIssueDate;
            SSNOExpireDate = ssnOExpireDate;
            PassportIssueDate = passportIssueDate;
            PassportExpireDate = passportExpireDate;
            AddressAsPerContract = addressAsPerContract;
            InsertRequestsForAnotherEmployee = insertRequestsForAnotherEmployee;
            IsSocialInsuranceIncluded = isSocialInsuranceIncluded;
            RegDate = DateTime.UtcNow;
        }

        public void Update(
            string? code = null,
            string? engName = null,
            string? arbName = null,
            string? arbName4S = null,
            string? familyEngName = null,
            string? familyArbName = null,
            string? familyArbName4S = null,
            string? fatherEngName = null,
            string? fatherArbName = null,
            string? fatherArbName4S = null,
            string? grandEngName = null,
            string? grandArbName = null,
            string? grandArbName4S = null,
            DateTime? birthDate = null,
            int? birthCityId = null,
            int? religionId = null,
            int? maritalStatusId = null,
            string? sex = null,
            int? bloodGroupId = null,
            int? bankId = null,
            int? nationalityId = null,
            string? bankAccountNumber = null,
            string? bankAccNumber = null,
            int? departmentId = null,
            string? gosiNumber = null,
            DateTime? gosiJoinDate = null,
            DateTime? gosiExcludeDate = null,
            DateTime? joinDate = null,
            DateTime? excludeDate = null,
            string? remarks = null,
            int? branchId = null,
            int? sponsorId = null,
            string? email = null,
            string? phone = null,
            string? mobile = null,
            int? managerId = null,
            string? machineCode = null,
            int? sectorId = null,
            string? ssnNo = null,
            string? passportNo = null,
            string? entryNo = null,
            int? cost1 = null,
            int? cost2 = null,
            int? cost3 = null,
            int? cost4 = null,
            string? laborOfficeNo = null,
            int? locationId = null,
            float? wHours = null,
            bool? isProjectRelated = null,
            bool? isSpecialForce = null,
            double? maxLoanDedution = null,
            string? ledgerCode = null,
            bool? hasTaqat = null,
            string? bankAccountType = null,
            bool? hasflexiblesalarydist = null,
            int? paymenttype = null,
            string? workEmail = null,
            string? ssnOIssueDate = null,
            string? ssnOExpireDate = null,
            string? passportIssueDate = null,
            string? passportExpireDate = null,
            string? addressAsPerContract = null,
            bool? insertRequestsForAnotherEmployee = null,
            bool? isSocialInsuranceIncluded = null)
        {
            if (code != null) Code = code;
            if (engName != null) EngName = engName;
            if (arbName != null) ArbName = arbName;
            if (arbName4S != null) ArbName4S = arbName4S;
            if (familyEngName != null) FamilyEngName = familyEngName;
            if (familyArbName != null) FamilyArbName = familyArbName;
            if (familyArbName4S != null) FamilyArbName4S = familyArbName4S;
            if (fatherEngName != null) FatherEngName = fatherEngName;
            if (fatherArbName != null) FatherArbName = fatherArbName;
            if (fatherArbName4S != null) FatherArbName4S = fatherArbName4S;
            if (grandEngName != null) GrandEngName = grandEngName;
            if (grandArbName != null) GrandArbName = grandArbName;
            if (grandArbName4S != null) GrandArbName4S = grandArbName4S;
            if (birthDate.HasValue) BirthDate = birthDate.Value;
            if (birthCityId.HasValue) BirthCityId = birthCityId.Value;
            if (religionId.HasValue) ReligionId = religionId.Value;
            if (maritalStatusId.HasValue) MaritalStatusId = maritalStatusId.Value;
            if (sex != null) Sex = sex;
            if (bloodGroupId.HasValue) BloodGroupId = bloodGroupId.Value;
            if (bankId.HasValue) BankId = bankId.Value;
            if (nationalityId.HasValue) NationalityId = nationalityId.Value;
            if (bankAccountNumber != null) BankAccountNumber = bankAccountNumber;
            if (bankAccNumber != null) BankAccNumber = bankAccNumber;
            if (departmentId.HasValue) DepartmentId = departmentId.Value;
            if (gosiNumber != null) GOSINumber = gosiNumber;
            if (gosiJoinDate.HasValue) GOSIJoinDate = gosiJoinDate.Value;
            if (gosiExcludeDate.HasValue) GOSIExcludeDate = gosiExcludeDate.Value;
            if (joinDate.HasValue) JoinDate = joinDate.Value;
            if (excludeDate.HasValue) ExcludeDate = excludeDate.Value;
            if (remarks != null) Remarks = remarks;
            if (branchId.HasValue) BranchId = branchId.Value;
            if (sponsorId.HasValue) SponsorId = sponsorId.Value;
            if (email != null) Email = email;
            if (phone != null) Phone = phone;
            if (mobile != null) Mobile = mobile;
            if (managerId.HasValue) ManagerId = managerId.Value;
            if (machineCode != null) MachineCode = machineCode;
            if (sectorId.HasValue) SectorId = sectorId.Value;
            if (ssnNo != null) SSnNo = ssnNo;
            if (passportNo != null) PassPortNo = passportNo;
            if (entryNo != null) EntryNo = entryNo;
            if (cost1.HasValue) Cost1 = cost1.Value;
            if (cost2.HasValue) Cost2 = cost2.Value;
            if (cost3.HasValue) Cost3 = cost3.Value;
            if (cost4.HasValue) Cost4 = cost4.Value;
            if (laborOfficeNo != null) LaborOfficeNo = laborOfficeNo;
            if (locationId.HasValue) LocationId = locationId.Value;
            if (wHours.HasValue) WHours = wHours.Value;
            if (isProjectRelated.HasValue) IsProjectRelated = isProjectRelated.Value;
            if (isSpecialForce.HasValue) IsSpecialForce = isSpecialForce.Value;
            if (maxLoanDedution.HasValue) MaxLoanDedution = maxLoanDedution.Value;
            if (ledgerCode != null) LedgerCode = ledgerCode;
            if (hasTaqat.HasValue) HasTaqat = hasTaqat.Value;
            if (bankAccountType != null) BankAccountType = bankAccountType;
            if (hasflexiblesalarydist.HasValue) Hasflexiblesalarydist = hasflexiblesalarydist.Value;
            if (paymenttype.HasValue) Paymenttype = paymenttype.Value;
            if (workEmail != null) WorkEmail = workEmail;
            if (ssnOIssueDate != null) SSNOIssueDate = ssnOIssueDate;
            if (ssnOExpireDate != null) SSNOExpireDate = ssnOExpireDate;
            if (passportIssueDate != null) PassportIssueDate = passportIssueDate;
            if (passportExpireDate != null) PassportExpireDate = passportExpireDate;
            if (addressAsPerContract != null) AddressAsPerContract = addressAsPerContract;
            if (insertRequestsForAnotherEmployee.HasValue) InsertRequestsForAnotherEmployee = insertRequestsForAnotherEmployee.Value;
            if (isSocialInsuranceIncluded.HasValue) IsSocialInsuranceIncluded = isSocialInsuranceIncluded.Value;
            UpdateUserId = RegUserId;
            UpdateDate = DateTime.Now;
        }

        public void Cancel( )
        {
            CancelDate = DateTime.Now;
        
        }

        public bool IsActive() => !CancelDate.HasValue;


        public string GetFullName(int lang)
        {
            var firstName = lang == 2 ? ArbName : EngName;
            var fatherName = lang == 2 ? FatherArbName : FatherEngName;
            var familyName = lang == 2 ? FamilyArbName : FamilyEngName;

            var arabicName = !string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(fatherName) || !string.IsNullOrEmpty(familyName);
            var englishName = !string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(fatherName) || !string.IsNullOrEmpty(familyName);

            if (lang == 2) // Arabic
            {
                if (arabicName)
                    return $"{firstName} {fatherName} {familyName}".Trim();
                else
                    return $"{EngName} {FatherEngName} {FamilyEngName}".Trim();
            }
            else // English
            {
                if (englishName)
                    return $"{firstName} {fatherName} {familyName}".Trim();
                else
                    return $"{EngName} {FatherEngName} {FamilyEngName}".Trim();
            }
        }
    }
}