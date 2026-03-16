using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Company.Dtos;
using Application.System.MasterData.Company.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Company.Commands
{
    public static class CreateCompany
    {
        public record Command(CreateCompanyDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateCompanyValidator());
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ICompanyRepository _repo;

            public Handler(ICompanyRepository repo)
            {
                _repo = repo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var exists = await _repo.CodeExistsAsync(request.Data.Code);
                if (exists)
                    throw new Exception($"Company with code '{request.Data.Code}' already exists");

                var company = new Domain.System.MasterData.Company(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    isHigry: request.Data.IsHigry,
                    includeAbsencDays: request.Data.IncludeAbsencDays,
                    empFirstName: request.Data.EmpFirstName,
                    empSecondName: request.Data.EmpSecondName,
                    empThirdName: request.Data.EmpThirdName,
                    empFourthName: request.Data.EmpFourthName,
                    empNameSeparator: request.Data.EmpNameSeparator,
                    remarks: request.Data.Remarks,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.RegComputerId,
                    prepareDay: request.Data.PrepareDay,
                    defaultTheme: request.Data.DefaultTheme,
                    vacationIsAccum: request.Data.VacationIsAccum,
                    hasSequence: request.Data.HasSequence,
                    sequenceLength: request.Data.SequenceLength,
                    prefix: request.Data.Prefix,
                    separator: request.Data.Separator,
                    salaryCalculation: request.Data.SalaryCalculation,
                    defaultAttend: request.Data.DefaultAttend,
                    countEmployeeVacationDaysTotal: request.Data.CountEmployeeVacationDaysTotal,
                    zeroBalAfterVac: request.Data.ZeroBalAfterVac,
                    vacSettlement: request.Data.VacSettlement,
                    allowOverVacation: request.Data.AllowOverVacation,
                    vacationFromPrepareDay: request.Data.VacationFromPrepareDay,
                    execuseRequestHoursallowed: request.Data.ExecuseRequestHoursallowed,
                    employeeDocumentsAutoSerial: request.Data.EmployeeDocumentsAutoSerial,
                    userDepartmentsPermissions: request.Data.UserDepartmentsPermissions
                );

                await _repo.AddAsync(company);
                await _repo.SaveChangesAsync(cancellationToken);

                return company.Id;
            }
        }
    }
}