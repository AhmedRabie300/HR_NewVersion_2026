using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Company.Dtos;
using Application.System.MasterData.Company.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Company.Commands
{
    public static class UpdateCompany
    {
        public record Command(UpdateCompanyDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateCompanyValidator(localizer, languageService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICompanyRepository _repo;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(ICompanyRepository repo, ILanguageService languageService, ILocalizationService localizer)
            {
                _repo = repo;
                _languageService = languageService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _languageService.GetCurrentLanguage();

                var company = await _repo.GetByIdAsync(request.Data.Id);
                if (company == null)
                    throw new NotFoundException("Update Company",string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        request.Data.Id));

                // Update basic info
               
                    company.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks,
                        request.Data.DefaultTheme
                    );
                

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