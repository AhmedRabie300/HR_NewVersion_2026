using Application.System.MasterData.Profession.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Profession.Validators
{
    public class CreateProfessionValidator : AbstractValidator<CreateProfessionDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly ILanguageService _languageService;

        public CreateProfessionValidator(ILocalizationService localizer, ILanguageService languageService)
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

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 2048));
        }
    }
}