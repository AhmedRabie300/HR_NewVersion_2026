using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Company.Dtos;
using MediatR;

namespace Application.System.MasterData.Company.Queries
{
    public static class ListCompanies
    {
        public record Query : IRequest<List<CompanyDto>>;

        public class Handler : IRequestHandler<Query, List<CompanyDto>>
        {
            private readonly ICompanyRepository _repo;

            public Handler(ICompanyRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<CompanyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companies = await _repo.GetAllAsync();

                return companies.Select(c => new CompanyDto(
                    Id: c.Id,
                    Code: c.Code,
                    EngName: c.EngName,
                    ArbName: c.ArbName,
                    ArbName4S: c.ArbName4S,
                    IsHigry: c.IsHigry,
                    IncludeAbsencDays: c.IncludeAbsencDays,
                    EmpFirstName: c.EmpFirstName,
                    EmpSecondName: c.EmpSecondName,
                    EmpThirdName: c.EmpThirdName,
                    EmpFourthName: c.EmpFourthName,
                    EmpNameSeparator: c.EmpNameSeparator,
                    Remarks: c.Remarks,
                    PrepareDay: c.PrepareDay,
                    DefaultTheme: c.DefaultTheme,
                    VacationIsAccum: c.VacationIsAccum,
                    HasSequence: c.HasSequence,
                    SequenceLength: c.SequenceLength,
                    Prefix: c.Prefix,
                    Separator: c.Separator,
                    SalaryCalculation: c.SalaryCalculation,
                    DefaultAttend: c.DefaultAttend,
                    CountEmployeeVacationDaysTotal: c.CountEmployeeVacationDaysTotal,
                    ZeroBalAfterVac: c.ZeroBalAfterVac,
                    VacSettlement: c.VacSettlement,
                    AllowOverVacation: c.AllowOverVacation,
                    VacationFromPrepareDay: c.VacationFromPrepareDay,
                    ExecuseRequestHoursallowed: c.ExecuseRequestHoursallowed,
                    EmployeeDocumentsAutoSerial: c.EmployeeDocumentsAutoSerial,
                    UserDepartmentsPermissions: c.UserDepartmentsPermissions,
                    RegDate: c.RegDate,
                    CancelDate: c.CancelDate,
                    IsActive: c.IsActive()
                )).ToList();
            }
        }
    }
}