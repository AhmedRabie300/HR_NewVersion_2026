using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Employees.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.Employees.Queries
{
    public static class GetEmployeeById
    {
        public record Query(int Id) : IRequest<EmployeeDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
    
            }
        }

        public class Handler : IRequestHandler<Query, EmployeeDto>
        {
            private readonly IEmployeeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IEmployeeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<EmployeeDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByIdAsync(request.Id);
              
                return new EmployeeDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    OldCode: entity.OldCode,
                    FullName:entity.GetFullName(lang),
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    FamilyEngName: entity.FamilyEngName,
                    FamilyArbName: entity.FamilyArbName,
                    FamilyArbName4S: entity.FamilyArbName4S,
                    FatherEngName: entity.FatherEngName,
                    FatherArbName: entity.FatherArbName,
                    FatherArbName4S: entity.FatherArbName4S,
                    GrandEngName: entity.GrandEngName,
                    GrandArbName: entity.GrandArbName,
                    GrandArbName4S: entity.GrandArbName4S,
                    BirthDate: entity.BirthDate,
                    BirthCityId: entity.BirthCityId,
                    BirthCityName: lang == 2 ? entity.BirthCity?.ArbName : entity.BirthCity?.EngName,
                    ReligionId: entity.ReligionId,
                    ReligionName: lang == 2 ? entity.Religion?.ArbName : entity.Religion?.EngName,
                    MaritalStatusId: entity.MaritalStatusId,
                    MaritalStatusName: lang == 2 ? entity.MaritalStatus?.ArbName : entity.MaritalStatus?.EngName,
                    Sex: entity.Sex,
                    BloodGroupId: entity.BloodGroupId,
                    BloodGroupName: lang == 2 ? entity.BloodGroup?.ArbName : entity.BloodGroup?.EngName,
                    BankId: entity.BankId,
                    BankName: lang == 2 ? entity.Bank?.ArbName : entity.Bank?.EngName,
                    NationalityId: entity.NationalityId,
                    NationalityName: lang == 2 ? entity.Nationality?.ArbName : entity.Nationality?.EngName,
                    BankAccountNumber: entity.BankAccountNumber,
                    BankAccNumber: entity.BankAccNumber,
                    DepartmentId: entity.DepartmentId,
                    DepartmentName: lang == 2 ? entity.Department?.ArbName : entity.Department?.EngName,
                    GOSINumber: entity.GOSINumber,
                    GOSIJoinDate: entity.GOSIJoinDate,
                    GOSIExcludeDate: entity.GOSIExcludeDate,
                    JoinDate: entity.JoinDate,
                    ExcludeDate: entity.ExcludeDate,
                    CompanyId: entity.CompanyId,
                    CompanyName: lang == 2 ? entity.Company?.ArbName : entity.Company?.EngName,
                    Remarks: entity.Remarks,
                    RegUserId: entity.RegUserId,
                    RegComputerId: entity.RegComputerId,
                    BranchId: entity.BranchId,
                    BranchName: lang == 2 ? entity.Branch?.ArbName : entity.Branch?.EngName,
                    SponsorId: entity.SponsorId,
                    SponsorName: lang == 2 ? entity.Sponsor?.ArbName : entity.Sponsor?.EngName,
                    Email: entity.Email,
                    Phone: entity.Phone,
                    Mobile: entity.Mobile,
                    ManagerId: entity.ManagerId,
                    ManagerName:entity.Manager.GetFullName(lang),
                    MachineCode: entity.MachineCode,
                    SectorId: entity.SectorId,
                    SectorName: lang == 2 ? entity.Sector?.ArbName : entity.Sector?.EngName,
                    SSnNo: entity.SSnNo,
                    PassPortNo: entity.PassPortNo,
                    EntryNo: entity.EntryNo,
                    Cost1: entity.Cost1,
                    Cost2: entity.Cost2,
                    Cost3: entity.Cost3,
                    Cost4: entity.Cost4,
                    LaborOfficeNo: entity.LaborOfficeNo,
                    LocationId: entity.LocationId,
                    LocationName: lang == 2 ? entity.Location?.ArbName : entity.Location?.EngName,
                    WHours: entity.WHours,
                    IsProjectRelated: entity.IsProjectRelated,
                    IsSpecialForce: entity.IsSpecialForce,
                    MaxLoanDedution: entity.MaxLoanDedution,
                    LedgerCode: entity.LedgerCode,
                    HasTaqat: entity.HasTaqat,
                    BankAccountType: entity.BankAccountType,
                    Hasflexiblesalarydist: entity.Hasflexiblesalarydist,
                    Paymenttype: entity.Paymenttype,
                    WorkEmail: entity.WorkEmail,
                    SSNOIssueDate: entity.SSNOIssueDate,
                    SSNOExpireDate: entity.SSNOExpireDate,
                    PassportIssueDate: entity.PassportIssueDate,
                    PassportExpireDate: entity.PassportExpireDate,
                    AddressAsPerContract: entity.AddressAsPerContract,
                    InsertRequestsForAnotherEmployee: entity.InsertRequestsForAnotherEmployee,
                    IsSocialInsuranceIncluded: entity.IsSocialInsuranceIncluded,
                    UpdateUserId: entity.UpdateUserId,
                    UpdateDate: entity.UpdateDate,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}