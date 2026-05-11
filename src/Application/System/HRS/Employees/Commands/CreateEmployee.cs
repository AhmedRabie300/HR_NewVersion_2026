using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Employees.Dtos;
using Application.System.HRS.Basics.Employees.Validators;
using Application.System.MasterData.Abstractions;
using Domain.System.HRS.Employees;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.Employees.Commands
{
    public static class CreateEmployee
    {
        public record Command(CreateEmployeeDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEmployeeRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateEmployeeValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IEmployeeRepository _repo;
            private readonly ICurrentUser _currentUser;
            private readonly ICompanyRepository _companyRepo;

            public Handler(IEmployeeRepository repo, ICurrentUser currentUser, ICompanyRepository companyRepo)
            {
                _repo = repo;
                _currentUser = currentUser;
                _companyRepo = companyRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;
                var regUserId = _currentUser.UserId ?? 0;

                // Get company settings  
                var company = await _companyRepo.GetByIdAsync(companyId);
          

                var prefixType = company.Prefix ?? 0;
                var separator = company.Separator ?? "";
                var padLength = company.SequenceLength ?? 5;

                // Determine the reference ID based on prefix type
                int? referenceId = prefixType switch
                {
                    1 => request.Data.BranchId,        // Branch
                    2 => request.Data.DepartmentId,    // Department
                    3 => request.Data.PositionId,      // Position
                    4 => request.Data.ContractTypeId,  // Contract Type
                    _ => null
                };

                // Get next code
                string code;
                if (string.IsNullOrWhiteSpace(request.Data.Code))
                {
                    code = await _repo.GetNextCodeAsync(prefixType, referenceId, separator, padLength, cancellationToken);
                    //if (string.IsNullOrEmpty(code))
                    //    throw new Exception("Failed to generate employee code");
                }
                else
                {
                    code = request.Data.Code.Trim();
                }

                // Get manager ID from manager code
                int? managerId = null;
                if (!string.IsNullOrWhiteSpace(request.Data.ManagerCode))
                {
                    var manager = await _repo.GetManagerByCodeAsync(request.Data.ManagerCode);
                    if (manager != null)
                        managerId = manager.Id;
                }

                var entity = new Employee(
                    code: code,
                    companyId: companyId,
                    regUserId: regUserId,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    familyEngName: request.Data.FamilyEngName,
                    familyArbName: request.Data.FamilyArbName,
                    familyArbName4S: request.Data.FamilyArbName4S,
                    fatherEngName: request.Data.FatherEngName,
                    fatherArbName: request.Data.FatherArbName,
                    fatherArbName4S: request.Data.FatherArbName4S,
                    grandEngName: request.Data.GrandEngName,
                    grandArbName: request.Data.GrandArbName,
                    grandArbName4S: request.Data.GrandArbName4S,
                    birthDate: request.Data.BirthDate,
                    birthCityId: request.Data.BirthCityId,
                    religionId: request.Data.ReligionId,
                    maritalStatusId: request.Data.MaritalStatusId,
                    sex: request.Data.Sex,
                    bloodGroupId: request.Data.BloodGroupId,
                    bankId: request.Data.BankId,
                    nationalityId: request.Data.NationalityId,
                    bankAccountNumber: request.Data.BankAccountNumber,
                    bankAccNumber: request.Data.BankAccNumber,
                    departmentId: request.Data.DepartmentId,
                    gosiNumber: request.Data.GOSINumber,
                    gosiJoinDate: request.Data.GOSIJoinDate,
                    gosiExcludeDate: request.Data.GOSIExcludeDate,
                    joinDate: request.Data.JoinDate,
                    excludeDate: request.Data.ExcludeDate,
                    remarks: request.Data.Remarks,
                    branchId: request.Data.BranchId,
                    sponsorId: request.Data.SponsorId,
                    email: request.Data.Email,
                    phone: request.Data.Phone,
                    mobile: request.Data.Mobile,
                    managerId: managerId,
                    machineCode: request.Data.MachineCode,
                    sectorId: request.Data.SectorId,
                    ssnNo: request.Data.SSnNo,
                    passportNo: request.Data.PassPortNo,
                    entryNo: request.Data.EntryNo,
                    cost1: request.Data.Cost1,
                    cost2: request.Data.Cost2,
                    cost3: request.Data.Cost3,
                    cost4: request.Data.Cost4,
                    laborOfficeNo: request.Data.LaborOfficeNo,
                    locationId: request.Data.LocationId,
                    wHours: request.Data.WHours,
                    isProjectRelated: request.Data.IsProjectRelated,
                    isSpecialForce: request.Data.IsSpecialForce,
                    maxLoanDedution: request.Data.MaxLoanDedution,
                    ledgerCode: request.Data.LedgerCode,
                    hasTaqat: request.Data.HasTaqat,
                    bankAccountType: request.Data.BankAccountType,
                    hasflexiblesalarydist: request.Data.Hasflexiblesalarydist,
                    paymenttype: request.Data.Paymenttype,
                    workEmail: request.Data.WorkEmail,
                    ssnOIssueDate: request.Data.SSNOIssueDate,
                    ssnOExpireDate: request.Data.SSNOExpireDate,
                    passportIssueDate: request.Data.PassportIssueDate,
                    passportExpireDate: request.Data.PassportExpireDate,
                    addressAsPerContract: request.Data.AddressAsPerContract,
                    insertRequestsForAnotherEmployee: request.Data.InsertRequestsForAnotherEmployee,
                    isSocialInsuranceIncluded: request.Data.IsSocialInsuranceIncluded
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}