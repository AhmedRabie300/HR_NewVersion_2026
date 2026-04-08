using Application.System.MasterData.Sponsor.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Sponsor.Validators
{
    public class UpdateSponsorValidator : AbstractValidator<UpdateSponsorDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly ILanguageService _languageService;

        public UpdateSponsorValidator(ILocalizationService localizer, ILanguageService languageService)
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

            RuleFor(x => x.SponsorNumber)
                .GreaterThan(0).When(x => x.SponsorNumber.HasValue)
                .WithMessage(x => _localizer.GetMessage("SponsorNumberMustBePositive", _languageService.GetCurrentLanguage()));

          
            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => _localizer.GetMessage("AtLeastOneField", _languageService.GetCurrentLanguage()));
        }

        private bool HaveAtLeastOneField(UpdateSponsorDto dto)
        {
            return dto.EngName != null || dto.ArbName != null ||
                   dto.ArbName4S != null || dto.SponsorNumber.HasValue;
         }
    }
}