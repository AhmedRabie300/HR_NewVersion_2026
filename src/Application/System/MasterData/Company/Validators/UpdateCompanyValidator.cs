using Application.System.MasterData.Company.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Company.Validators
{
    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly ILanguageService _languageService;

        public UpdateCompanyValidator(ILocalizationService localizer, ILanguageService languageService)
        {
            _localizer = localizer;
            _languageService = languageService;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 100));

            RuleFor(x => x.EmpFirstName)
                .MaximumLength(10).When(x => x.EmpFirstName != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 10));

            RuleFor(x => x.EmpSecondName)
                .MaximumLength(10).When(x => x.EmpSecondName != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 10));

            RuleFor(x => x.EmpThirdName)
                .MaximumLength(10).When(x => x.EmpThirdName != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 10));

            RuleFor(x => x.EmpFourthName)
                .MaximumLength(10).When(x => x.EmpFourthName != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 10));

            RuleFor(x => x.EmpNameSeparator)
                .MaximumLength(1).When(x => x.EmpNameSeparator != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 1));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 2048));

            RuleFor(x => x.DefaultTheme)
                .MaximumLength(50).When(x => x.DefaultTheme != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 50));

            RuleFor(x => x.SequenceLength)
                .GreaterThan(0).When(x => x.SequenceLength.HasValue)
                .WithMessage(x => _localizer.GetMessage("SequenceLengthMustBePositive", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.Prefix)
                .GreaterThan(0).When(x => x.Prefix.HasValue)
                .WithMessage(x => _localizer.GetMessage("PrefixMustBePositive", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.Separator)
                .MaximumLength(1).When(x => x.Separator != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 1));

            RuleFor(x => x.PrepareDay)
                .InclusiveBetween(1, 31).When(x => x.PrepareDay.HasValue)
                .WithMessage(x => _localizer.GetMessage("PrepareDayRange", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.ExecuseRequestHoursallowed)
                .GreaterThan(0).When(x => x.ExecuseRequestHoursallowed.HasValue)
                .WithMessage(x => _localizer.GetMessage("ExecuseRequestHoursPositive", _languageService.GetCurrentLanguage()));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => _localizer.GetMessage("AtLeastOneField", _languageService.GetCurrentLanguage()));
        }

        private bool HaveAtLeastOneField(UpdateCompanyDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.IsHigry.HasValue ||
                   dto.IncludeAbsencDays.HasValue ||
                   dto.EmpFirstName != null ||
                   dto.EmpSecondName != null ||
                   dto.EmpThirdName != null ||
                   dto.EmpFourthName != null ||
                   dto.EmpNameSeparator != null ||
                   dto.Remarks != null ||
                   dto.PrepareDay.HasValue ||
                   dto.DefaultTheme != null ||
                   dto.VacationIsAccum.HasValue ||
                   dto.HasSequence.HasValue ||
                   dto.SequenceLength.HasValue ||
                   dto.Prefix.HasValue ||
                   dto.Separator != null ||
                   dto.SalaryCalculation.HasValue ||
                   dto.DefaultAttend.HasValue ||
                   dto.CountEmployeeVacationDaysTotal.HasValue ||
                   dto.ZeroBalAfterVac.HasValue ||
                   dto.VacSettlement.HasValue ||
                   dto.AllowOverVacation.HasValue ||
                   dto.VacationFromPrepareDay.HasValue ||
                   dto.ExecuseRequestHoursallowed.HasValue ||
                   dto.EmployeeDocumentsAutoSerial.HasValue ||
                   dto.UserDepartmentsPermissions.HasValue;
        }
    }
}