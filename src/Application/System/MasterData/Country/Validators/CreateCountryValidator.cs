using Application.Common.Abstractions;
using Application.System.MasterData.Country.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Country.Validators
{
    public class CreateCountryValidator : AbstractValidator<CreateCountryDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _contextService;

        public CreateCountryValidator(ILocalizationService localizer, IContextService contextService)
        {
            _localizer = localizer;
            _contextService = contextService;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(255).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 255));

            RuleFor(x => x.ArbName)
                .MaximumLength(255).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 255));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(255).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 255));

            RuleFor(x => x.PhoneKey)
                .MaximumLength(50).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.ISOAlpha2)
                .MaximumLength(2).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 2));

            RuleFor(x => x.ISOAlpha3)
                .MaximumLength(3).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 3));

            RuleFor(x => x.Languages)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Continent)
                .MaximumLength(10).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0).When(x => x.CurrencyId.HasValue)
                .WithMessage(_localizer.GetMessage("CurrencyRequired", lang));

            RuleFor(x => x.NationalityId)
                .GreaterThan(0).When(x => x.NationalityId.HasValue)
                .WithMessage(_localizer.GetMessage("NationalityRequired", lang));

            RuleFor(x => x.RegionId)
                .GreaterThan(0).When(x => x.RegionId.HasValue)
                .WithMessage(_localizer.GetMessage("RegionRequired", lang));

            RuleFor(x => x.CapitalId)
                .GreaterThan(0).When(x => x.CapitalId.HasValue)
                .WithMessage(_localizer.GetMessage("CapitalRequired", lang));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}