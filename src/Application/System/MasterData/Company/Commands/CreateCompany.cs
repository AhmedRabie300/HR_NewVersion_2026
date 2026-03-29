// Application/System/MasterData/Company/Commands/CreateCompany.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Company.Dtos;
using Application.System.MasterData.Company.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Company.Commands
{
    public static class CreateCompany
    {
        public record Command(CreateCompanyDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILocalizationService _localizer;

            // Constructor واحد بس
            public Validator(IHttpContextAccessor httpContextAccessor, ILocalizationService localizer)
            {
                _httpContextAccessor = httpContextAccessor;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .Custom((data, context) =>
                    {
                        var lang = GetLanguage();
                        var validator = new CreateCompanyValidator(_localizer, lang);
                        var result = validator.Validate(data);
                        if (!result.IsValid)
                        {
                            foreach (var error in result.Errors)
                            {
                                context.AddFailure(error);
                            }
                        }
                    });
            }

            private int GetLanguage()
            {
                var context = _httpContextAccessor.HttpContext;
                if (context != null && context.Items.ContainsKey("Language"))
                {
                    return (int)context.Items["Language"]!;
                }
                return 1; // Default English
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ICompanyRepository _repo;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILocalizationService _localizer;

            public Handler(ICompanyRepository repo, IHttpContextAccessor httpContextAccessor, ILocalizationService localizer)
            {
                _repo = repo;
                _httpContextAccessor = httpContextAccessor;
                _localizer = localizer;
            }

            private int GetLanguage()
            {
                var context = _httpContextAccessor.HttpContext;
                if (context != null && context.Items.ContainsKey("Language"))
                {
                    return (int)context.Items["Language"]!;
                }
                return 1;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = GetLanguage();

                var exists = await _repo.CodeExistsAsync(request.Data.Code);
                if (exists)
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("Company", lang),
                        request.Data.Code));

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
                    regComputerId: request.Data.regComputerId,
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