using Application.System.MasterData.Company.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Company.Validators
{
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly ILanguageService _languageService;

        public CreateCompanyValidator(ILocalizationService localizer, ILanguageService languageService)
        {
            _localizer = localizer;
            _languageService = languageService;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => _localizer.GetMessage("CodeRequired", _languageService.GetCurrentLanguage()))
                .MaximumLength(50).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 100));

            RuleFor(x => x.EmpFirstName)
                .MaximumLength(10).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 10));

            RuleFor(x => x.EmpSecondName)
                .MaximumLength(10).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 10));

            RuleFor(x => x.EmpThirdName)
                .MaximumLength(10).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 10));

            RuleFor(x => x.EmpFourthName)
                .MaximumLength(10).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 10));

            RuleFor(x => x.EmpNameSeparator)
                .MaximumLength(1).When(x => !string.IsNullOrEmpty(x.EmpNameSeparator))
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 1));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 2048));

            RuleFor(x => x.DefaultTheme)
                .MaximumLength(50).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 50));

            RuleFor(x => x.SequenceLength)
                .GreaterThan(0).When(x => x.SequenceLength.HasValue)
                .WithMessage(x => _localizer.GetMessage("SequenceLengthMustBePositive", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.Prefix)
                .GreaterThan(0).When(x => x.Prefix.HasValue)
                .WithMessage(x => _localizer.GetMessage("PrefixMustBePositive", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.Separator)
                .MaximumLength(1).When(x => !string.IsNullOrEmpty(x.Separator))
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 1));

            RuleFor(x => x.PrepareDay)
                .InclusiveBetween(1, 31).When(x => x.PrepareDay.HasValue)
                .WithMessage(x => _localizer.GetMessage("PrepareDayRange", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.ExecuseRequestHoursallowed)
                .GreaterThan(0).When(x => x.ExecuseRequestHoursallowed.HasValue)
                .WithMessage(x => _localizer.GetMessage("ExecuseRequestHoursPositive", _languageService.GetCurrentLanguage()));
        }
    }
}