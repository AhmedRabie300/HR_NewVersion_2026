using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Company.Dtos;
using Application.System.MasterData.Company.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Common.Abstractions;

namespace Application.System.MasterData.Company.Commands
{
    public static class UpdateCompany
    {
        public record Command(UpdateCompanyDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILocalizationService _localizer;

            public Validator(IHttpContextAccessor httpContextAccessor, ILocalizationService localizer)
            {
                _httpContextAccessor = httpContextAccessor;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .Custom((data, context) =>
                    {
                        var lang = GetLanguage();
                        var validator = new UpdateCompanyValidator(_localizer, lang);
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
                return 1;
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
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

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = GetLanguage();

                var company = await _repo.GetByIdAsync(request.Data.Id);
                if (company == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        request.Data.Id));

                // Update basic info
                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Remarks != null ||
                    request.Data.DefaultTheme != null)
                {
                    company.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks,
                        request.Data.DefaultTheme
                    );
                }

                // Update employee naming
                if (request.Data.EmpFirstName != null ||
                    request.Data.EmpSecondName != null ||
                    request.Data.EmpThirdName != null ||
                    request.Data.EmpFourthName != null ||
                    request.Data.EmpNameSeparator != null)
                {
                    company.UpdateEmployeeNaming(
                        request.Data.EmpFirstName,
                        request.Data.EmpSecondName,
                        request.Data.EmpThirdName,
                        request.Data.EmpFourthName,
                        request.Data.EmpNameSeparator
                    );
                }

                // Update vacation settings
                if (request.Data.VacationIsAccum.HasValue ||
                    request.Data.ZeroBalAfterVac.HasValue ||
                    request.Data.VacSettlement.HasValue ||
                    request.Data.AllowOverVacation.HasValue ||
                    request.Data.VacationFromPrepareDay.HasValue ||
                    request.Data.ExecuseRequestHoursallowed.HasValue)
                {
                    company.UpdateVacationSettings(
                        request.Data.VacationIsAccum,
                        request.Data.ZeroBalAfterVac,
                        request.Data.VacSettlement,
                        request.Data.AllowOverVacation,
                        request.Data.VacationFromPrepareDay,
                        request.Data.ExecuseRequestHoursallowed
                    );
                }

                // Update sequence settings
                if (request.Data.HasSequence.HasValue ||
                    request.Data.SequenceLength.HasValue ||
                    request.Data.Prefix.HasValue ||
                    request.Data.Separator != null)
                {
                    company.UpdateSequenceSettings(
                        request.Data.HasSequence,
                        request.Data.SequenceLength,
                        request.Data.Prefix,
                        request.Data.Separator
                    );
                }

                // Update flags
                if (request.Data.IsHigry.HasValue ||
                    request.Data.IncludeAbsencDays.HasValue ||
                    request.Data.DefaultAttend.HasValue ||
                    request.Data.CountEmployeeVacationDaysTotal.HasValue ||
                    request.Data.EmployeeDocumentsAutoSerial.HasValue ||
                    request.Data.UserDepartmentsPermissions.HasValue)
                {
                    company.UpdateFlags(
                        request.Data.IsHigry,
                        request.Data.IncludeAbsencDays,
                        request.Data.DefaultAttend,
                        request.Data.CountEmployeeVacationDaysTotal,
                        request.Data.EmployeeDocumentsAutoSerial,
                        request.Data.UserDepartmentsPermissions
                    );
                }

                // Update salary calculation
                if (request.Data.SalaryCalculation.HasValue)
                {
                    company.UpdateSalaryCalculation(request.Data.SalaryCalculation);
                }

                // Update prepare day
                if (request.Data.PrepareDay.HasValue)
                {
                    company.UpdatePrepareDay(request.Data.PrepareDay);
                }

                await _repo.UpdateAsync(company);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}