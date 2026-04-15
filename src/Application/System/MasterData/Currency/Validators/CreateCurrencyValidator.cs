using Application.Common.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Currency.Validators
{
    public class CreateCurrencyValidator : AbstractValidator<CreateCurrencyDto>
    {
        public CreateCurrencyValidator(ILocalizationService localizer, IContextService contextService)
        {
            var lang = contextService.GetCurrentLanguage();

            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.EngSymbol)
                .MaximumLength(10).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.ArbSymbol)
                .MaximumLength(10).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.DecimalFraction)
                .GreaterThanOrEqualTo(0).WithMessage(localizer.GetMessage("DecimalFractionRequired", lang));

            RuleFor(x => x.DecimalEngName)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.DecimalArbName)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.NoDecimalPlaces)
                .InclusiveBetween(0, 4).When(x => x.NoDecimalPlaces.HasValue)
                .WithMessage(localizer.GetMessage("DecimalPlacesRange", lang));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}