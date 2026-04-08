// Application/System/MasterData/Education/Validators/CreateEducationValidator.cs
using Application.System.MasterData.Education.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Education.Validators
{
    public class CreateEducationValidator : AbstractValidator<CreateEducationDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly ILanguageService _languageService;

        public CreateEducationValidator(ILocalizationService localizer, ILanguageService languageService)
        {
            _localizer = localizer;
            _languageService = languageService;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => _localizer.GetMessage("CodeRequired", _languageService.GetCurrentLanguage()))
                .MaximumLength(50).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 50));

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage(x => _localizer.GetMessage("CompanyRequired", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 100));

            RuleFor(x => x.Level)
                .GreaterThan(0).When(x => x.Level.HasValue)
                .WithMessage(x => _localizer.GetMessage("LevelMustBePositive", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.RequiredYears)
                .GreaterThan(0).When(x => x.RequiredYears.HasValue)
                .WithMessage(x => _localizer.GetMessage("RequiredYearsMustBePositive", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 2048));
        }
    }
}