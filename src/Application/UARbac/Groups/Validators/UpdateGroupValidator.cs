using Application.Common.Abstractions;
using Application.UARbac.Groups.Dtos;
using FluentValidation;

namespace Application.UARbac.Groups.Validators
{
    public class UpdateGroupValidator : AbstractValidator<UpdateGroupDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly ILanguageService _languageService;
        public UpdateGroupValidator(ILocalizationService localizer, ILanguageService languageService)
        {
            _localizer = localizer;
            _languageService = languageService;
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(x => _localizer.GetMessage("Valid Group ID is required", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.EngName)
                .MaximumLength(200)
                .WithMessage(x => _localizer.GetMessage("English name must not exceed 200 characters", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.ArbName)
                .MaximumLength(200)
                .WithMessage(x => _localizer.GetMessage("Arabic name must not exceed 200 characters", _languageService.GetCurrentLanguage()));

            RuleFor(x => x)
                .Must(x => x.EngName != null || x.ArbName != null)
                .WithMessage(x => _localizer.GetMessage("At least one field must be provided to update", _languageService.GetCurrentLanguage()));
        }
    }
}