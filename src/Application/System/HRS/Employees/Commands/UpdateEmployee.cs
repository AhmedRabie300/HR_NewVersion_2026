using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.Employees.Validators;
using Application.System.HRS.Employees.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.Employees.Commands
{
    public static class UpdateEmployee
    {
        public record Command(UpdateEmployeeDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEmployeeRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateEmployeeValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IEmployeeRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IEmployeeRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Employee", request.Data.Id));

                // Get manager ID from manager code if provided
                int? managerId = null;
                if (request.Data.ManagerCode != null)
                {
                    if (!string.IsNullOrWhiteSpace(request.Data.ManagerCode))
                    {
                        var manager = await _repo.GetManagerByCodeAsync(request.Data.ManagerCode);
                        if (manager != null)
                            managerId = manager.Id;
                    }
                }
                else
                {
                    managerId = entity.ManagerId; // Keep existing
                }

                entity.Update(
                    code: request.Data.Code,
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

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}