using Application.System.MasterData.Nationality.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Nationality.Validators
{
    public class UpdateNationalityValidator : AbstractValidator<UpdateNationalityDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly ILanguageService _languageService;

        public UpdateNationalityValidator(ILocalizationService localizer, ILanguageService languageService)
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

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 2048));

            RuleFor(x => x.TravelRoute)
                .GreaterThan(0).When(x => x.TravelRoute.HasValue)
                .WithMessage(x => _localizer.GetMessage("TravelRouteMustBePositive", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.TravelClass)
                .GreaterThan(0).When(x => x.TravelClass.HasValue)
                .WithMessage(x => _localizer.GetMessage("TravelClassMustBePositive", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.TicketAmount)
                .GreaterThan(0).When(x => x.TicketAmount.HasValue)
                .WithMessage(x => _localizer.GetMessage("TicketAmountMustBePositive", _languageService.GetCurrentLanguage()));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => _localizer.GetMessage("AtLeastOneField", _languageService.GetCurrentLanguage()));
        }

        private bool HaveAtLeastOneField(UpdateNationalityDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.IsMainNationality.HasValue ||
                   dto.TravelRoute.HasValue ||
                   dto.TravelClass.HasValue ||
                   dto.Remarks != null ||
                   dto.TicketAmount.HasValue;
        }
    }
}