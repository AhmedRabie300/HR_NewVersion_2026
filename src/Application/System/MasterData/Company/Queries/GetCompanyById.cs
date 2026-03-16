using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Company.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Company.Queries
{
    public static class GetCompanyById
    {
        public record Query(int Id) : IRequest<CompanyDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("Company ID must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, CompanyDto>
        {
            private readonly ICompanyRepository _repo;

            public Handler(ICompanyRepository repo)
            {
                _repo = repo;
            }

            public async Task<CompanyDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var company = await _repo.GetByIdAsync(request.Id);
                if (company == null)
                    throw new Exception($"Company with ID {request.Id} not found");

                return new CompanyDto(
                    Id: company.Id,
                    Code: company.Code,
                    EngName: company.EngName,
                    ArbName: company.ArbName,
                    ArbName4S: company.ArbName4S,
                    IsHigry: company.IsHigry,
                    IncludeAbsencDays: company.IncludeAbsencDays,
                    EmpFirstName: company.EmpFirstName,
                    EmpSecondName: company.EmpSecondName,
                    EmpThirdName: company.EmpThirdName,
                    EmpFourthName: company.EmpFourthName,
                    EmpNameSeparator: company.EmpNameSeparator,
                    Remarks: company.Remarks,
                    PrepareDay: company.PrepareDay,
                    DefaultTheme: company.DefaultTheme,
                    VacationIsAccum: company.VacationIsAccum,
                    HasSequence: company.HasSequence,
                    SequenceLength: company.SequenceLength,
                    Prefix: company.Prefix,
                    Separator: company.Separator,
                    SalaryCalculation: company.SalaryCalculation,
                    DefaultAttend: company.DefaultAttend,
                    CountEmployeeVacationDaysTotal: company.CountEmployeeVacationDaysTotal,
                    ZeroBalAfterVac: company.ZeroBalAfterVac,
                    VacSettlement: company.VacSettlement,
                    AllowOverVacation: company.AllowOverVacation,
                    VacationFromPrepareDay: company.VacationFromPrepareDay,
                    ExecuseRequestHoursallowed: company.ExecuseRequestHoursallowed,
                    EmployeeDocumentsAutoSerial: company.EmployeeDocumentsAutoSerial,
                    UserDepartmentsPermissions: company.UserDepartmentsPermissions,
                    RegDate: company.RegDate,
                    CancelDate: company.CancelDate,
                    IsActive: company.IsActive()
                );
            }
        }
    }
}